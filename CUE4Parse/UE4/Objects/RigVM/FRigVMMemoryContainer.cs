using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Objects.RigVM;

public class FRigVMMemoryContainer
{
	public bool bUseNameMap;

	public ERigVMMemoryType MemoryType;

	public FRigVMRegister[] Registers;

	public FRigVMRegisterOffset[] RegisterOffsets;

	public string[] ScriptStructPaths;

	public ulong TotalBytes;

	public object View;

	public FRigVMMemoryContainer(FAssetArchive Ar)
	{
		bUseNameMap = Ar.ReadBoolean();
		MemoryType = Ar.Read<ERigVMMemoryType>();
		Registers = Ar.ReadArray(() => new FRigVMRegister(Ar));
		RegisterOffsets = Ar.ReadArray(() => new FRigVMRegisterOffset(Ar));
		ScriptStructPaths = Ar.ReadArray(Ar.ReadFString);
		TotalBytes = Ar.Read<ulong>();
		FRigVMRegister[] registers = Registers;
		foreach (FRigVMRegister fRigVMRegister in registers)
		{
			if (fRigVMRegister.ElementCount == 0 && !fRigVMRegister.IsDynamic())
			{
				continue;
			}
			if (!fRigVMRegister.IsDynamic() || !fRigVMRegister.IsNestedDynamic())
			{
				switch (fRigVMRegister.Type)
				{
				case ERigVMRegisterType.Plain:
					View = Ar.ReadArray<byte>();
					break;
				case ERigVMRegisterType.Name:
					View = Ar.ReadArray(Ar.ReadFName);
					break;
				case ERigVMRegisterType.String:
				case ERigVMRegisterType.Struct:
					View = Ar.ReadArray(Ar.ReadFString);
					break;
				}
				continue;
			}
			for (int num2 = 0; num2 < fRigVMRegister.SliceCount; num2++)
			{
				switch (fRigVMRegister.Type)
				{
				case ERigVMRegisterType.Plain:
					View = Ar.ReadArray<byte>();
					break;
				case ERigVMRegisterType.Name:
					View = Ar.ReadArray(Ar.ReadFName);
					break;
				case ERigVMRegisterType.String:
				case ERigVMRegisterType.Struct:
					View = Ar.ReadArray(Ar.ReadFString);
					break;
				}
			}
		}
	}
}
