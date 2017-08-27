using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class TextureExporter {
	
	[MenuItem("CONTEXT/SpriteRenderer/Export texture to png")]
	private static void ExportSpriteRendererTextureToPng(MenuCommand command)
	{
		var spriteRenderer = (SpriteRenderer)command.context;
		var sprite = spriteRenderer.sprite;
		ExportTexture (sprite.texture,sprite.textureRect,sprite.name);
	}

	[MenuItem("CONTEXT/RawImage/Export texture to png")]
	private static void ExportRawImageTextureToPng(MenuCommand command)
	{
		var Image = (RawImage)command.context;
		var texture = Image.texture;
		ExportTexture (texture,new Rect(0,0,texture.width,texture.height),texture.name);
	}

	[MenuItem("CONTEXT/Image/Export texture to png")]
	private static void ExportImageTextureToPng(MenuCommand command)
	{
		var Image = (Image)command.context;
		var sprite = Image.sprite;
		ExportTexture (sprite.texture,sprite.textureRect,sprite.name);
	}

	[MenuItem("CONTEXT/MeshRenderer/Export main texture to png")]
	private static void ExportMeshRendererMainTextureToPng(MenuCommand command)
	{
		var meshRenderer = (MeshRenderer)command.context;
		var texture = meshRenderer.sharedMaterial.mainTexture;
		ExportTexture (texture,new Rect(0,0,texture.width,texture.height),texture.name);
	}

	private static void ExportTexture(Texture sourceTexture,Rect textureRect, string name) {
		
		RenderTexture rt = RenderTexture.GetTemporary(sourceTexture.width,sourceTexture.height,0,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Default);
		Graphics.Blit(sourceTexture,rt);

		RenderTexture.active = rt;

		Texture2D readableTexture = new Texture2D((int)textureRect.width,(int)textureRect.height,TextureFormat.ARGB32,false);
		readableTexture.ReadPixels(textureRect,0,0 );

		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(rt);
		var pngBytes = readableTexture.EncodeToPNG();

		var savePath = EditorUtility.SaveFilePanel("Save image", "", name, "png");

		if (string.IsNullOrEmpty(savePath))
		{
			return;
		}

		File.WriteAllBytes(savePath,pngBytes);
	}
}
