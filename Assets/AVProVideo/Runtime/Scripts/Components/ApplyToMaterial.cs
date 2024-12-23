//-----------------------------------------------------------------------------
// Copyright 2015-2024 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_VISIONOS
	#define UNITY_PLATFORM_SUPPORTS_YPCBCR
#endif

using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	/// <summary>
	/// Sets up a material to display the video from a MediaPlayer
	/// </summary>
	[AddComponentMenu("AVPro Video/Apply To Material", 300)]
	[HelpURL("https://www.renderheads.com/products/avpro-video/")]
	public sealed class ApplyToMaterial : ApplyToBase
	{
		[Header("Display")]

		[Tooltip("Default texture to display when the video texture is preparing")]
		[SerializeField]
		Texture2D _defaultTexture = null;

		public Texture2D DefaultTexture
		{
			get
			{
				return _defaultTexture;
			}
			set
			{
				if (_defaultTexture != value)
				{
					_defaultTexture = value;
					_isDirty = true;
				}
			}
		}

		[Space(8f)]
		[Header("Material Target")]

		[SerializeField]
		Material _material = null;

		public Material Material
		{
			get
			{
				return _material;
			}
			set
			{
				if (_material != value)
				{ 
					_material = value;
					_isDirty = true;
				}
			}
		}

		[SerializeField]
		string _texturePropertyName = Helper.UnityBaseTextureName;

		public string TexturePropertyName
		{
			get
			{
				return _texturePropertyName;
			}
			set
			{
				if (_texturePropertyName != value)
				{
					_texturePropertyName = value;
					_propTexture = new LazyShaderProperty(_texturePropertyName);
					_propTexture_R = new LazyShaderProperty(_texturePropertyName + "_R");
					_isDirty = true;
				}
			}
		}

		[SerializeField]
		Vector2 _offset = Vector2.zero;

		public Vector2 Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				if (_offset != value)
				{
					_offset = value;
					_isDirty = true;
				}
			}
		}

		[SerializeField]
		Vector2 _scale = Vector2.one;

		public Vector2 Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				if (_scale != value)
				{
					_scale = value;
					_isDirty = true;
				}
			}
		}

		private Texture _lastTextureApplied;
		private LazyShaderProperty _propTexture;
		private LazyShaderProperty _propTexture_R;	// Default property for the right-eye texture

		private Texture _originalTexture;
		private Vector2 _originalScale = Vector2.one;
		private Vector2 _originalOffset = Vector2.zero;

		// We do a LateUpdate() to allow for any changes in the texture that may have happened in Update()
		private void LateUpdate()
		{
			Apply();
		}

		public override void Apply()
		{
			bool applied = false;

			if (_media != null && _media.TextureProducer != null)
			{
				Texture resamplerTex = _media.FrameResampler == null || _media.FrameResampler.OutputTexture == null ? null : _media.FrameResampler.OutputTexture[0];
				Texture texture = _media.UseResampler ? resamplerTex : _media.TextureProducer.GetTexture(0);
				if (texture != null)
				{
					// Check for changing texture
					if (texture != _lastTextureApplied)
					{
						_isDirty = true;
					}

					if (_isDirty)
					{
						bool requiresVerticalFlip = _media.TextureProducer.RequiresVerticalFlip();
						StereoPacking stereoPacking = _media.TextureProducer.GetTextureStereoPacking();
						
						int planeCount = 1;
						if (!_media.UseResampler)
						{
							// We're not using the resampler so the number of planes will be the texture count
							planeCount = _media.TextureProducer.GetTextureCount();
							if (stereoPacking == StereoPacking.TwoTextures)
							{
								// Unless we're using two texture stereo in which case it'll be half the texture count
								planeCount /= 2;
							}
						}

						for (int plane = 0; plane < planeCount; ++plane)
						{
							Texture resamplerTexPlane = _media.FrameResampler == null || _media.FrameResampler.OutputTexture == null ? null : _media.FrameResampler.OutputTexture[plane];
							texture = _media.UseResampler ? resamplerTexPlane : _media.TextureProducer.GetTexture(plane);
							if (texture != null)
							{
								ApplyMapping(texture, requiresVerticalFlip, plane);
							}
						}

						// Handle the right eye if we're using two texture stereo packing
						if (stereoPacking == StereoPacking.TwoTextures)
						{
							for (int plane = 0; plane < planeCount; ++plane)
							{
								texture = _media.TextureProducer.GetTexture(planeCount + plane);
								if (texture != null)
								{
									ApplyMapping(texture, requiresVerticalFlip, plane, Eye.Right);
								}
							}
						}
					}

					applied = true;
				}
			}

			// If the media didn't apply a texture, then try to apply the default texture
			if (!applied)
			{
				if (_defaultTexture != _lastTextureApplied)
				{
					_isDirty = true;
				}
				if (_isDirty)
				{
#if UNITY_PLATFORM_SUPPORTS_YPCBCR
					if (_material != null && _material.HasProperty(VideoRender.PropUseYpCbCr.Id))
					{
						_material.DisableKeyword(VideoRender.Keyword_UseYpCbCr);
					}
#endif
					ApplyMapping(_defaultTexture, false);
				}
			}
		}

		enum Eye
		{
			Left,
			Right
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="requiresYFlip"></param>
		/// <param name="plane"></param>
		/// <param name="eye">Which eye we're mapping, defaults to the left eye</param>
		private void ApplyMapping(Texture texture, bool requiresYFlip, int plane = 0, Eye eye = Eye.Left)
		{
			if (_material != null)
			{
				_isDirty = false;

				if (plane == 0)
				{
					int propTextureId = _propTexture.Id;
					if (eye == Eye.Left)
					{
						VideoRender.SetupMaterialForMedia(_material, _media, propTextureId, texture, texture == _defaultTexture);
						_lastTextureApplied = texture;
						#if !UNITY_EDITOR && UNITY_ANDROID
							if (texture == _defaultTexture)
							{
								_material.EnableKeyword("USING_DEFAULT_TEXTURE");
							}
							else
							{
								_material.DisableKeyword("USING_DEFAULT_TEXTURE");
							}
						#endif
					}
					else
					{
						propTextureId = _propTexture_R.Id;
						_material.SetTexture(propTextureId, texture);
					}

					if (texture != null)
					{
						if (requiresYFlip)
						{
							_material.SetTextureScale(propTextureId, new Vector2(_scale.x, -_scale.y));
							_material.SetTextureOffset(propTextureId, Vector2.up + _offset);
						}
						else
						{
							_material.SetTextureScale(propTextureId, _scale);
							_material.SetTextureOffset(propTextureId, _offset);
						}
					}
				}
				else if (plane == 1)
				{
					if (texture != null)
					{
						if (requiresYFlip)
						{
							_material.SetTextureScale(VideoRender.PropChromaTex.Id, new Vector2(_scale.x, -_scale.y));
							_material.SetTextureOffset(VideoRender.PropChromaTex.Id, Vector2.up + _offset);
						}
						else
						{
							_material.SetTextureScale(VideoRender.PropChromaTex.Id, _scale);
							_material.SetTextureOffset(VideoRender.PropChromaTex.Id, _offset);
						}
					}
				}
			}
		}

		protected override void SaveProperties()
		{
			if (_material != null)
			{
				if (string.IsNullOrEmpty(_texturePropertyName))
				{
					_originalTexture = _material.mainTexture;
					_originalScale = _material.mainTextureScale;
					_originalOffset = _material.mainTextureOffset;
				}
				else
				{
					_originalTexture = _material.GetTexture(_texturePropertyName);
					_originalScale = _material.GetTextureScale(_texturePropertyName);
					_originalOffset = _material.GetTextureOffset(_texturePropertyName);
				}
			}
			_propTexture = new LazyShaderProperty(_texturePropertyName);
			_propTexture_R = new LazyShaderProperty(_texturePropertyName + "_R");
		}

		protected override void RestoreProperties()
		{
			if (_material != null)
			{
				if (string.IsNullOrEmpty(_texturePropertyName))
				{
					_material.mainTexture = _originalTexture;
					_material.mainTextureScale = _originalScale;
					_material.mainTextureOffset = _originalOffset;
				}
				else
				{
					_material.SetTexture(_texturePropertyName, _originalTexture);
					_material.SetTextureScale(_texturePropertyName, _originalScale);
					_material.SetTextureOffset(_texturePropertyName, _originalOffset);
				}
			}
		}
	}
}
