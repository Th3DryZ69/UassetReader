namespace CUE4Parse.UE4.Objects.RigVM;

public readonly struct FRigVMJumpIfOp
{
	public readonly ERigVMOpCode OpCode;

	public readonly FRigVMOperand Arg;

	public readonly int InstructionIndex;

	public readonly bool Condition;
}
