// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public enum ASESRPVersions
	{
		ASE_SRP_4_1_0 = 040100,
		ASE_SRP_4_2_0 = 040200,
		ASE_SRP_4_3_0 = 040300,
		ASE_SRP_4_6_0 = 040600,
		ASE_SRP_4_8_0 = 040800,
		ASE_SRP_4_9_0 = 040900,
		ASE_SRP_4_10_0 = 041000
	}

	[Serializable]
	public class ASEPackageManagerHelper
	{
		private const string HDPackageId = "com.unity.render-pipelines.high-definition";
		private const string LightweigthId = "com.unity.render-pipelines.lightweight";

		private const string SPKeywordFormat = "ASE_SRP_VERSION {0}";
		private SearchRequest m_packageHDSearchRequest = null;
		private SearchRequest m_packageLWSearchRequest = null;
		private ListRequest m_packageListRequest = null;
		// V4.8.0 and bellow
		// HD 
		private readonly string[] GetNormalWSFunc =
		{
			"inline void GetNormalWS( FragInputs input, float3 normalTS, out float3 normalWS, float3 doubleSidedConstants )\n",
			"{\n",
			"\tGetNormalWS( input, normalTS, normalWS );\n",
			"}\n"
		};

		// v4.6.0 and bellow
		private readonly string[] BuildWordTangentFunc =
		{
			"float3x3 BuildWorldToTangent(float4 tangentWS, float3 normalWS)\n",
			"{\n",
			"\tfloat3 unnormalizedNormalWS = normalWS;\n",
			"\tfloat renormFactor = 1.0 / length(unnormalizedNormalWS);\n",
			"\tfloat3x3 worldToTangent = CreateWorldToTangent(unnormalizedNormalWS, tangentWS.xyz, tangentWS.w > 0.0 ? 1.0 : -1.0);\n",
			"\tworldToTangent[0] = worldToTangent[0] * renormFactor;\n",
			"\tworldToTangent[1] = worldToTangent[1] * renormFactor;\n",
			"\tworldToTangent[2] = worldToTangent[2] * renormFactor;\n",
			"\treturn worldToTangent;\n",
			"}\n"
		};

		///////////
		private bool m_requireUpdateLW = true;
		private bool m_requireUpdateHD = true;
		private bool m_requireUpdateList = true;

		[SerializeField]
		private ASESRPVersions m_currentHDVersion = ASESRPVersions.ASE_SRP_4_10_0;

		[SerializeField]
		private ASESRPVersions m_currentLWVersion = ASESRPVersions.ASE_SRP_4_10_0;

		private Dictionary<string, ASESRPVersions> m_srpVersionConverter = new Dictionary<string, ASESRPVersions>()
		{
			{"4.1.0-preview",   ASESRPVersions.ASE_SRP_4_1_0},
			{"4.2.0-preview",   ASESRPVersions.ASE_SRP_4_2_0},
			{"4.3.0-preview",   ASESRPVersions.ASE_SRP_4_3_0},
			{"4.6.0-preview",   ASESRPVersions.ASE_SRP_4_6_0},
			{"4.8.0-preview",   ASESRPVersions.ASE_SRP_4_8_0},
			{"4.9.0-preview",   ASESRPVersions.ASE_SRP_4_9_0},
			{"4.10.0-preview",   ASESRPVersions.ASE_SRP_4_10_0},
		};


		public void RequestInfo()
		{
			m_requireUpdateLW = true;
			m_requireUpdateHD = true;
			m_requireUpdateList = true;
			m_packageHDSearchRequest = UnityEditor.PackageManager.Client.Search( HDPackageId );
			m_packageLWSearchRequest = UnityEditor.PackageManager.Client.Search( LightweigthId );
			m_packageListRequest = UnityEditor.PackageManager.Client.List( true );
		}

		public void Update()
		{
			if( m_requireUpdateLW )
			{
				if( m_packageLWSearchRequest != null )
				{
					m_requireUpdateLW = !m_packageLWSearchRequest.IsCompleted;
				}
			}

			if( m_requireUpdateHD )
			{
				if( m_packageHDSearchRequest != null )
				{
					m_requireUpdateHD = !m_packageHDSearchRequest.IsCompleted;
				}
			}

			if( m_requireUpdateList )
			{
				if( m_packageListRequest != null && m_packageListRequest.IsCompleted )
				{
					m_requireUpdateList = !m_packageListRequest.IsCompleted;
					foreach( UnityEditor.PackageManager.PackageInfo pi in m_packageListRequest.Result )
					{
						if( pi.name.Equals( LightweigthId ) )
						{
							if( m_srpVersionConverter.ContainsKey( pi.version ) )
							{
								m_currentLWVersion = m_srpVersionConverter[ pi.version ];
							}
							//else
							//{
							//	UIUtils.ShowMessage( "Unrecognized Lightweight Version" );
							//}
						}

						if( pi.name.Equals( HDPackageId ) )
						{
							if( m_srpVersionConverter.ContainsKey( pi.version ) )
							{
								m_currentHDVersion = m_srpVersionConverter[ pi.version ];
							}
							//else
							//{
							//	UIUtils.ShowMessage( "Unrecognized HD Version" );
							//}
						}
					}
				}
			}
		}

		public void SetSRPInfoOnDataCollector( ref MasterNodeDataCollector dataCollector )
		{
			if( m_requireUpdateLW || m_requireUpdateHD || m_requireUpdateList )
				Update();

			if( dataCollector.CurrentSRPType == TemplateSRPType.HD )
			{
				dataCollector.AddToDefines( -1, string.Format( SPKeywordFormat, (int)m_currentHDVersion ) );
				if( m_currentHDVersion < ASESRPVersions.ASE_SRP_4_9_0 )
				{
					dataCollector.AddFunction( GetNormalWSFunc[ 0 ], GetNormalWSFunc, false );
				}

				if( m_currentHDVersion < ASESRPVersions.ASE_SRP_4_8_0 )
				{
					dataCollector.AddFunction( BuildWordTangentFunc[ 0 ], BuildWordTangentFunc, false );
				}
			}

			if( dataCollector.CurrentSRPType == TemplateSRPType.Lightweight )
				dataCollector.AddToDefines( -1, string.Format( SPKeywordFormat, (int)m_currentLWVersion ) );
		}
		public ASESRPVersions CurrentHDVersion { get { return m_currentHDVersion; } }
		public ASESRPVersions CurrentLWVersion { get { return m_currentLWVersion; } }
	}
}
#endif
