using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.RigVM;

public class FRigVMByteCode
{
	public readonly int InstructionCount;

	public readonly ERigVMOpCode OpCode;

	public FRigVMByteCode(FArchive Ar)
	{
		InstructionCount = Ar.Read<int>();
		OpCode = Ar.Read<ERigVMOpCode>();
		switch (OpCode)
		{
		case ERigVMOpCode.Execute_0_Operands:
		case ERigVMOpCode.Execute_1_Operands:
		case ERigVMOpCode.Execute_2_Operands:
		case ERigVMOpCode.Execute_3_Operands:
		case ERigVMOpCode.Execute_4_Operands:
		case ERigVMOpCode.Execute_5_Operands:
		case ERigVMOpCode.Execute_6_Operands:
		case ERigVMOpCode.Execute_7_Operands:
		case ERigVMOpCode.Execute_8_Operands:
		case ERigVMOpCode.Execute_9_Operands:
		case ERigVMOpCode.Execute_10_Operands:
		case ERigVMOpCode.Execute_11_Operands:
		case ERigVMOpCode.Execute_12_Operands:
		case ERigVMOpCode.Execute_13_Operands:
		case ERigVMOpCode.Execute_14_Operands:
		case ERigVMOpCode.Execute_15_Operands:
		case ERigVMOpCode.Execute_16_Operands:
		case ERigVMOpCode.Execute_17_Operands:
		case ERigVMOpCode.Execute_18_Operands:
		case ERigVMOpCode.Execute_19_Operands:
		case ERigVMOpCode.Execute_20_Operands:
		case ERigVMOpCode.Execute_21_Operands:
		case ERigVMOpCode.Execute_22_Operands:
		case ERigVMOpCode.Execute_23_Operands:
		case ERigVMOpCode.Execute_24_Operands:
		case ERigVMOpCode.Execute_25_Operands:
		case ERigVMOpCode.Execute_26_Operands:
		case ERigVMOpCode.Execute_27_Operands:
		case ERigVMOpCode.Execute_28_Operands:
		case ERigVMOpCode.Execute_29_Operands:
		case ERigVMOpCode.Execute_30_Operands:
		case ERigVMOpCode.Execute_31_Operands:
		case ERigVMOpCode.Execute_32_Operands:
		case ERigVMOpCode.Execute_33_Operands:
		case ERigVMOpCode.Execute_34_Operands:
		case ERigVMOpCode.Execute_35_Operands:
		case ERigVMOpCode.Execute_36_Operands:
		case ERigVMOpCode.Execute_37_Operands:
		case ERigVMOpCode.Execute_38_Operands:
		case ERigVMOpCode.Execute_39_Operands:
		case ERigVMOpCode.Execute_40_Operands:
		case ERigVMOpCode.Execute_41_Operands:
		case ERigVMOpCode.Execute_42_Operands:
		case ERigVMOpCode.Execute_43_Operands:
		case ERigVMOpCode.Execute_44_Operands:
		case ERigVMOpCode.Execute_45_Operands:
		case ERigVMOpCode.Execute_46_Operands:
		case ERigVMOpCode.Execute_47_Operands:
		case ERigVMOpCode.Execute_48_Operands:
		case ERigVMOpCode.Execute_49_Operands:
		case ERigVMOpCode.Execute_50_Operands:
		case ERigVMOpCode.Execute_51_Operands:
		case ERigVMOpCode.Execute_52_Operands:
		case ERigVMOpCode.Execute_53_Operands:
		case ERigVMOpCode.Execute_54_Operands:
		case ERigVMOpCode.Execute_55_Operands:
		case ERigVMOpCode.Execute_56_Operands:
		case ERigVMOpCode.Execute_57_Operands:
		case ERigVMOpCode.Execute_58_Operands:
		case ERigVMOpCode.Execute_59_Operands:
		case ERigVMOpCode.Execute_60_Operands:
		case ERigVMOpCode.Execute_61_Operands:
		case ERigVMOpCode.Execute_62_Operands:
		case ERigVMOpCode.Execute_63_Operands:
		case ERigVMOpCode.Execute_64_Operands:
		{
			byte b = (byte)(Ar.Read<FRigVMExecuteOp>().OpCode - 0);
			for (int i = 0; i < b; i++)
			{
				Ar.Read<FRigVMOperand>();
			}
			break;
		}
		case ERigVMOpCode.Copy:
			Ar.Read<FRigVMCopyOp>();
			break;
		case ERigVMOpCode.Zero:
		case ERigVMOpCode.BoolFalse:
		case ERigVMOpCode.BoolTrue:
		case ERigVMOpCode.Increment:
		case ERigVMOpCode.Decrement:
			Ar.Read<FRigVMUnaryOp>();
			break;
		case ERigVMOpCode.Equals:
		case ERigVMOpCode.NotEquals:
			Ar.Read<FRigVMComparisonOp>();
			break;
		case ERigVMOpCode.JumpAbsolute:
		case ERigVMOpCode.JumpForward:
		case ERigVMOpCode.JumpBackward:
			Ar.Read<FRigVMJumpOp>();
			break;
		case ERigVMOpCode.JumpAbsoluteIf:
		case ERigVMOpCode.JumpForwardIf:
		case ERigVMOpCode.JumpBackwardIf:
			Ar.Read<FRigVMJumpIfOp>();
			break;
		case ERigVMOpCode.BeginBlock:
			Ar.Read<FRigVMBinaryOp>();
			break;
		case ERigVMOpCode.ChangeType:
		case ERigVMOpCode.Exit:
			break;
		}
	}
}
