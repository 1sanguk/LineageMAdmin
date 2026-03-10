namespace LineageMOps.Models.Domain;

/// <summary>혈맹원 활동 기록 점수 (활동 기록 탭)</summary>
public class ClanActivityScore
{
    public int Pve       { get; set; }  // PVE 점수
    public int Pvp       { get; set; }  // PVP 점수
    public int Boss      { get; set; }  // 보스 토벌 점수
    public int Pk        { get; set; }  // PK 점수
    public int LoginRate { get; set; }  // 최근 30일 접속 일수 (0~30)
    public int Chat      { get; set; }  // 채팅 발송 횟수
    public int Enchant   { get; set; }  // 인챈트 시도 횟수
    public int Summon    { get; set; }  // 소환 점수
    public int Trade     { get; set; }  // 거래 점수
    public int SealQuest { get; set; }  // 인장 퀘스트 점수
}
