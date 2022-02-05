﻿using AssetRipper.Core.Classes;
using AssetRipper.Core.Classes.Meta;
using AssetRipper.Core.Interfaces;
using AssetRipper.Core.Project.Exporters;
using System;
using System.IO;

namespace AssetRipper.Core.Project.Collections
{
	public class ManagerExportCollection : AssetExportCollection
	{
		public ManagerExportCollection(IAssetExporter assetExporter, IUnityObjectBase asset) : this(assetExporter, (IGlobalGameManager)asset) { }

		public ManagerExportCollection(IAssetExporter assetExporter, IGlobalGameManager asset) : base(assetExporter, asset) { }

		public override bool Export(ProjectAssetContainer container, string dirPath)
		{
			string subPath = Path.Combine(dirPath, ProjectSettingsName);
			string name = GetCorrectName(Asset.GetType().Name);
			string fileName = $"{name}.asset";
			string filePath = Path.Combine(subPath, fileName);

			Directory.CreateDirectory(subPath);

			ExportInner(container, filePath);
			return true;
		}

		public override long GetExportID(IUnityObjectBase asset)
		{
			if (asset == Asset)
			{
				return 1;
			}
			throw new ArgumentException(nameof(asset));
		}

		public override MetaPtr CreateExportPointer(IUnityObjectBase asset, bool isLocal)
		{
			throw new NotSupportedException();
		}

		private static string GetCorrectName(string typeName)
		{
			return typeName switch
			{
				PlayerSettingsName => ProjectSettingsName,
				NavMeshProjectSettingsName => NavMeshAreasName,
				PhysicsManagerName => DynamicsManagerName,
				_ => typeName,
			};
		}

		//Type names
		private const string PlayerSettingsName = "PlayerSettings";
		private const string NavMeshProjectSettingsName = "NavMeshProjectSettings";
		private const string PhysicsManagerName = "PhysicsManager";

		//Altered names
		private const string ProjectSettingsName = "ProjectSettings";
		private const string NavMeshAreasName = "NavMeshAreas";
		private const string DynamicsManagerName = "DynamicsManager";
	}
}
