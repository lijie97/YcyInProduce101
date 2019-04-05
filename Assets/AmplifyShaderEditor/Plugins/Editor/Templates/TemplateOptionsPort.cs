// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using UnityEngine;

namespace AmplifyShaderEditor
{
	// PORT CONTROLLERS
	[Serializable]
	public class TemplateOptionPortItem
	{
		[SerializeField]
		private int m_portId = -1;

		[SerializeField]
		private TemplateOptionsItem m_options;

		public TemplateOptionPortItem( TemplateMultiPassMasterNode owner, TemplateOptionsItem options )
		{
			m_options = options;
			InputPort port = owner.InputPorts.Find( x => x.Name.Equals( options.Name ) );
			if( port != null )
			{
				m_portId = port.PortId;
			}
		}

		public void FillDataCollector( TemplateMultiPassMasterNode owner, ref MasterNodeDataCollector dataCollector )
		{
			InputPort port = null;
			if( m_portId > -1 )
			{
				port = owner.GetInputPortByUniqueId( m_portId );
			}
			else
			{
				port = owner.InputPorts.Find( x => x.Name.Equals( m_options.Name ) );
			}

			if( port != null )
			{
				int optionId = port.HasOwnOrLinkConnection ? 0 : 1;
				for( int i = 0; i < m_options.ActionsPerOption[ optionId ].Length; i++ )
				{
					switch( m_options.ActionsPerOption[ optionId ][ i ].ActionType )
					{
						case AseOptionsActionType.SetDefine:
						{
							dataCollector.AddToDefines( -1, m_options.ActionsPerOption[ optionId ][ i ].ActionData );
						}
						break;
						case AseOptionsActionType.SetUndefine:
						{
							dataCollector.AddToDefines( -1, m_options.ActionsPerOption[ optionId ][ i ].ActionData, false );
						}
						break;
					}
				}
			}

		}

		public TemplateOptionsItem Options { get { return m_options; } }
	}
}
