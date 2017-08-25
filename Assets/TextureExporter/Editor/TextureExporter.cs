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
		ExportTexture (spriteRenderer.sprite.texture);
	}

	[MenuItem("CONTEXT/RawImage/Export texture to png")]
	private static void ExportRawImageTextureToPng(MenuCommand command)
	{
		var Image = (RawImage)command.context;
		ExportTexture (Image.texture);
	}

	[MenuItem("CONTEXT/Image/Export texture to png")]
	private static void ExportImageTextureToPng(MenuCommand command)
	{
		var Image = (Image)command.context;
		ExportTexture (Image.sprite.texture);
	}

	[MenuItem("CONTEXT/MeshRenderer/Export main texture to png")]
	private static void ExportMeshRendererMainTextureToPng(MenuCommand command)
	{
		var meshRenderer = (MeshRenderer)command.context;
		ExportTexture (meshRenderer.sharedMaterial.mainTexture);
	}

	private static void ExportTexture(Texture sourceTexture) {
		
		RenderTexture rt = RenderTexture.GetTemporary(sourceTexture.width,sourceTexture.height,0,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Default);
		Graphics.Blit(sourceTexture,rt);

		RenderTexture.active = rt;

		Texture2D readableTexture = new Texture2D(sourceTexture.width,sourceTexture.height,TextureFormat.ARGB32,false);
		readableTexture.ReadPixels(new Rect(0,0,sourceTexture.width,sourceTexture.height),0,0 );

		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(rt);
		var pngBytes = readableTexture.EncodeToPNG();

		var savePath = EditorUtility.SaveFilePanel("Save image", "", sourceTexture.name, "png");

		if (string.IsNullOrEmpty(savePath))
		{
			return;
		}

		File.WriteAllBytes(savePath,pngBytes);
	}
}
