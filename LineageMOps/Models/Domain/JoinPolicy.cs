namespace LineageMOps.Models.Domain;

public enum JoinPolicy
{
    Closed,     // 거부
    Immediate,  // 즉시
    Approval,   // 승인
    Password    // 암호
}
