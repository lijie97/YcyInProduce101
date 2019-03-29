using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVFileTool
{
	protected List<List<string>> dataArray = new List<List<string>> ();
	/// <summary>
	/// 总列数.
	/// </summary>
	public int colCount;
	/// <summary>
	/// 总行数.
	/// </summary>
	public int rowCount;
	public string filePath;

	public CSVFileTool (string path)
	{
		this.filePath = path;
		OpenCSV ();
	}

	/// <summary>
	/// 获取某行某列的数据
	/// row:行,row = 1代表第一行
	/// col:列,col = 1代表第一列  
	/// </summary>
	/// <param name="row">Row.</param>
	/// <param name="col">Col.</param>
	public string this [int row, int col] {
		get {
			int dataRow = dataArray.Count;
			int dataCol = dataArray [0].Count;

			if (row < 0 || row >= dataRow || col < 1 || col > dataCol) {
                Debug.Log(row+" "+dataRow+" "+col+" "+dataCol);
				Debug.LogError ("out of range");
				return "";
			}
			
			return dataArray [row ] [col - 1];
		}
		set {
			int dataRow = dataArray.Count;
			int dataCol = dataArray [0].Count;

			if (row < 0 || row >= dataRow || col < 1 || col > dataCol) {
				Debug.LogError ("out of range");
				return ;
			}

			//Debug.Log(row + "  "+(col-1)+ "  "+value );
			dataArray [row ] [col - 1] = value;
		}
	}

	public string this [int row, string colName] {
		get {
			int dataRow = dataArray.Count;
			int dataCol = dataArray [0].Count;

			if (row < 0 || row >= dataRow) {
				Debug.LogError ("out of range");
				return "";
			}

			int index = dataArray [0].IndexOf (colName);
			if (index < 0) {
				Debug.LogError ("out of range");
				return "";
			}
			
			return dataArray [row ] [index].ToString ();
		}
		set {

			int dataRow = dataArray.Count;
			int dataCol = dataArray [0].Count;

			if (row < 0 || row >= dataRow) {
				Debug.Log ("out of range");
				return;
			}

			int index = dataArray [0].IndexOf (colName);
			if (index < 0) {
				Debug.Log ("out of range");
				return;
			}

			//Debug.Log(row + "  "+index+colName+ "  "+value );
			dataArray [row ] [index] = value;
		}
	}

	/// <summary>
	/// 将数据写入到CSV文件中
	/// </summary>
	/// <param name="dt">提供保存数据的DataTable</param>
	/// <param name="fileName">CSV的文件路径</param>
	public void SaveCSV ()
	{
		FileInfo fi = new FileInfo (filePath);
		if (!fi.Directory.Exists) {
			fi.Directory.Create ();
		}
		FileStream fs = new FileStream (filePath, FileMode.Create, FileAccess.Write);
		StreamWriter sw = new StreamWriter (fs, Encoding.UTF8);


		StringBuilder strBuilder = new StringBuilder ();

	
		for (int i = 0; i < dataArray.Count; ++i) {
			for (int j = 0; j < dataArray [0].Count; ++j) {

				strBuilder.Append (dataArray[i][j]);
				//Debug.Log(str);
				if (j < dataArray [0].Count - 1)
					strBuilder.Append (',');
			}

			if (i < dataArray.Count - 1)
				strBuilder.Append ('\r');
		}


		sw.Write (strBuilder);

		sw.Close ();
		fs.Close ();
	}

	/// <summary>
	/// 将CSV文件的数据读取到DataTable中
	/// </summary>
	/// <param name="fileName">CSV文件路径</param>
	/// <returns>返回读取了CSV数据的DataTable</returns>
	private void OpenCSV ()
	{
		FileStream fs = new FileStream (filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
		
		StreamReader sr = new StreamReader (fs, Encoding.UTF8);
		//记录每次读取的一行记录
		string strLine = "";
		//记录每行记录中的各字段内容
		string[] aryLine = null;
		string[] tableHead = null;
		//标示是否是读取的第一行
		bool IsFirst = true;
		rowCount = 0;
		//逐行读取CSV中的数据
		while ((strLine = sr.ReadLine ()) != null) {
			if (IsFirst == true) {
				tableHead = strLine.Split (',');
				IsFirst = false;
				colCount = tableHead.Length;

				List<string> headRow = new List<string> ();
				headRow.AddRange (tableHead);
				dataArray.Add (headRow);	//创建列
                rowCount++;
			} else {
				aryLine = strLine.Split (',');

				List<string> rowData = new List<string> ();
				rowData.AddRange (aryLine);
				dataArray.Add (rowData);
			
				rowCount++;
			}
			
		}
		sr.Close ();
		fs.Close ();
	}

	public List<string> GetRow (int index)
	{
		return dataArray [index];
	}

    public void InsertNewRow(int rowindex, List<string> newr)
    {
        dataArray.Insert(rowindex, newr);
        rowCount++;
    }

	public void AddNewRow (List<string> newr)
	{
		dataArray.Add (newr);
		rowCount++;
	}

	public void AddNewRow (params string[] strParams)
	{
		List<string> dataRow = new List<string> (strParams);
		dataArray.Add (dataRow);
		rowCount++;
	}

	public void RemoveRow (int index)
	{
		if (index < 0 || index >= dataArray.Count)
			return;
		
		dataArray.RemoveAt (index);
		rowCount--;
	}

    public void AddNewCol(string colname)
    {
        for (int i = 0; i < rowCount; i++)
        {
            if (i == 0)
            {
                dataArray[i].Add(colname);
            }
            else
            {
                dataArray[i].Add("");
            }
        }
        colCount ++;
    }

}

