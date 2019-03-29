using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class BMFont : EditorWindow
{
    private TextAsset _fontTextAsset;
    private Texture _fontTexture;
    private string _fontsDir;

    [MenuItem("CTools/BMFont", false, 12)]
    private static void BMFontTools()
    {
        BMFont bmFont = new BMFont();
        bmFont.Show();
    }

    private string _getAssetPath(string path)
    {
        string pathTemp = path.Replace("\\", "/");
        pathTemp = pathTemp.Replace(Application.dataPath, "Assets");
        return pathTemp;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        TextAsset taTemp = EditorGUILayout.ObjectField("选择Font文件：", _fontTextAsset, typeof(TextAsset), true) as TextAsset;
        if (taTemp != _fontTextAsset && taTemp != null)
        {
            string assetDir = Directory.GetParent(AssetDatabase.GetAssetPath(taTemp)).FullName;
            assetDir = _getAssetPath(assetDir);
            string imgPath = string.Format("{0}/{1}_0.png", assetDir, taTemp.name);
            _fontTexture = AssetDatabase.LoadAssetAtPath<Texture>(imgPath);
            _fontsDir = string.Format("{0}.fontsettings", Path.Combine(assetDir, taTemp.name));
            if (_fontTexture == null)
            {
                _fontsDir = string.Empty;
                Debug.LogError(string.Format("未发现{0}文件", imgPath));
            }
        }
        _fontTextAsset = taTemp;

        _fontTexture = EditorGUILayout.ObjectField("选择Font图片文件：", _fontTexture, typeof(Texture), true) as Texture;

        GUI.enabled = false;
        _fontsDir = EditorGUILayout.TextField("字体生成路径:", _fontsDir);
        GUI.enabled = true;
        if (GUILayout.Button("Generate Font"))
        {
            if (!string.IsNullOrEmpty(_fontsDir))
            {
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(_fontsDir.Replace(".fontsettings", ".mat"));
                if (mat == null)
                {
                    mat = new Material(Shader.Find("UI/Default Font"));
                    AssetDatabase.CreateAsset(mat, _fontsDir.Replace(".fontsettings", ".mat"));
                }
                if (_fontTexture != null)
                {
                    mat = AssetDatabase.LoadAssetAtPath<Material>(_fontsDir.Replace(".fontsettings", ".mat"));
                    mat.SetTexture("_MainTex", _fontTexture);
                }
                else
                {
                    Debug.LogError("贴图未做配置，请检查配置");
                    return;
                }

                Font font = AssetDatabase.LoadAssetAtPath<Font>(_fontsDir);
                if (font == null)
                {
                    font = new Font();
                    AssetDatabase.CreateAsset(font, _fontsDir);
                }

                _setFontInfo(AssetDatabase.LoadAssetAtPath<Font>(_fontsDir),
                    AssetDatabase.GetAssetPath(_fontTextAsset),
                    _fontTexture);
                font = AssetDatabase.LoadAssetAtPath<Font>(_fontsDir);
                font.material = mat;
            }
            else
            {
                Debug.LogError("创建失败，请检查配置");
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void _setFontInfo(Font font, string fontConfig, Texture texture)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(fontConfig);
        List<CharacterInfo> chtInfoList = new List<CharacterInfo>();
        XmlNode node = xml.SelectSingleNode("font/chars");

        XmlNode commonNode = xml.SelectSingleNode("font/common");
        XmlElement commonXe = (XmlElement)commonNode;
        int textureWidth = int.Parse(commonXe.GetAttribute("scaleW"));
        int textureHeight = int.Parse(commonXe.GetAttribute("scaleH"));

        foreach (XmlNode nd in node.ChildNodes)
        {
            XmlElement xe = (XmlElement)nd;
            int x = int.Parse(xe.GetAttribute("x"));
            int y = int.Parse(xe.GetAttribute("y"));
            int width = int.Parse(xe.GetAttribute("width"));
            int height = int.Parse(xe.GetAttribute("height"));
            int advance = int.Parse(xe.GetAttribute("xadvance"));
            int xoffset = int.Parse(xe.GetAttribute("xoffset"));
            int yoffset = int.Parse(xe.GetAttribute("yoffset"));
            CharacterInfo info = new CharacterInfo();
            //info.glyphHeight = texture.height;
            //info.glyphWidth = texture.width;
            //info.index = int.Parse(xe.GetAttribute("id"));
            ////这里注意下UV坐标系和从BMFont里得到的信息的坐标系是不一样的哦，前者左下角为（0,0），
            ////右上角为（1,1）。而后者则是左上角上角为（0,0），右下角为（图宽，图高）
            //info.uvTopLeft = new Vector2((float)x / texture.width, 1 - (float)y / texture.height);
            //info.uvTopRight = new Vector2((float)(x + width) / texture.width, 1 - (float)y / texture.height);
            //info.uvBottomLeft = new Vector2((float)x / texture.width, 1 - (float)(y + height) / texture.height);
            //info.uvBottomRight = new Vector2((float)(x + width) / texture.width, 1 - (float)(y + height) / texture.height);
            //info.minX = 0;
            //info.minY = -height;
            //info.maxX = width;
            //info.maxY = 0;
            //info.advance = advance;  上面这些是错误的
            
            info.index = int.Parse(xe.GetAttribute("id"));
            float uvx = 1f * x / textureWidth;
            float uvy = 1 - (1f * y / textureHeight);
            float uvw = 1f * width / textureWidth;
            float uvh = -1f * height / textureHeight;

            info.uvBottomLeft = new Vector2(uvx, uvy);
            info.uvBottomRight = new Vector2(uvx + uvw, uvy);
            info.uvTopLeft = new Vector2(uvx, uvy + uvh);
            info.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);

            info.minX = xoffset;
            info.minY = yoffset + height / 2; //除2使字体居中
            info.glyphWidth = width;
            info.glyphHeight = -height;
            info.advance = advance;

            chtInfoList.Add(info);
        }
        font.characterInfo = chtInfoList.ToArray();
        AssetDatabase.Refresh();
    }
}