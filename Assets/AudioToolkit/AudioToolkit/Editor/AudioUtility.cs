using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR // workaround for strange compiler error during build process

namespace UnityEditor
{
    public static class AudioUtility
    {

        public static void PlayClip( AudioClip clip )
        {
            Assembly unityEditorAssembly = typeof( AudioImporter ).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType( "UnityEditor.AudioUtil" );
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[ ] {
                typeof(AudioClip)
            },
            null
            );
            method.Invoke(
                null,
                new object[ ] {
                clip
            }
            );
        }

        public static void StopClip( AudioClip clip )
        {
            Assembly unityEditorAssembly = typeof( AudioImporter ).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType( "UnityEditor.AudioUtil" );
            MethodInfo method = audioUtilClass.GetMethod(
                "StopClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[ ] {
                typeof(AudioClip)
            },
            null
            );
            method.Invoke(
                null,
                new object[ ] {
                clip
            }
            );
        }

        public static void PauseClip( AudioClip clip )
        {
            Assembly unityEditorAssembly = typeof( AudioImporter ).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType( "UnityEditor.AudioUtil" );
            MethodInfo method = audioUtilClass.GetMethod(
                "PauseClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[ ] {
                typeof(AudioClip)
            },
            null
            );
            method.Invoke(
                null,
                new object[ ] {
                clip
            }
            );
        }

        public static void ResumeClip( AudioClip clip )
        {
            Assembly unityEditorAssembly = typeof( AudioImporter ).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType( "UnityEditor.AudioUtil" );
            MethodInfo method = audioUtilClass.GetMethod(
                "ResumeClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[ ] {
                typeof(AudioClip)
            },
            null
            );
            method.Invoke(
                null,
                new object[ ] {
                clip
            }
            );
        }
    }
}

#endif