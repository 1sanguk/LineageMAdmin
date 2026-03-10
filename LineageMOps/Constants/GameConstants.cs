using LineageMOps.Models.Domain;

namespace LineageMOps.Constants;

public static class GameConstants
{
    // 레거시 월드 서버 (서버 번호 01~10 생략)
    public static readonly string[] LegacyServers =
    {
        "판도라", "라스타바드",
        "듀크데필", "블루디카",
        "파푸리온", "질리언",
        "린드비오르", "사이하",
        "발라카스", "군터",
        "안타라스", "하딘",
        "데스나이트", "아툰",
        "켄라우헬", "케레니스",
        "기르타스", "진기르타스",
        "그림리퍼", "발록",
    };

    // 리부트 월드 서버 (서버 번호 01~10 생략)
    public static readonly string[] RebootServers =
    {
        "말하는섬", "윈다우드",
        "글루디오", "그레시아",
    };

    // 전체 서버 목록 (레거시 + 리부트)
    public static readonly string[] ServerNames =
        [.. LegacyServers, .. RebootServers];

    public static readonly IReadOnlyDictionary<CharacterClass, string> ClassDisplayNames =
        new Dictionary<CharacterClass, string>
        {
            [CharacterClass.Lord]       = "군주",
            [CharacterClass.Knight]     = "기사",
            [CharacterClass.Elf]        = "요정",
            [CharacterClass.Wizard]     = "마법사",
            [CharacterClass.DarkElf]    = "다크엘프",
            [CharacterClass.Gunner]     = "총사",
            [CharacterClass.Fighter]    = "투사",
            [CharacterClass.DarkKnight] = "암흑기사",
            [CharacterClass.HolyKnight] = "신성검사",
            [CharacterClass.Berserker]  = "광전사",
            [CharacterClass.Reaper]     = "사신",
            [CharacterClass.ThunderGod] = "뇌신",
            [CharacterClass.SpellBlade] = "마검사",
        };

    public static string GetClassName(CharacterClass cls)
        => ClassDisplayNames.TryGetValue(cls, out var name) ? name : cls.ToString();
}

public static class ClanConstants
{
    public const int MembersPerLevel           = 10;
    public const int MaxMemberCapacity         = 100;
    public const int BloodOathMembersPerLevel  = 5;   // 피의 서약 레벨당 최대 인원 증가
    public const int BloodOathMaxLevel         = 6;
    public const int AcademyMinLevel           = 5;   // 아카데미 개설 최소 혈맹 레벨
    public const int AcademyMaxCapacity        = 20;  // 아카데미 최대 정원
    public const int AcademyGraduationLevel    = 40;  // 아카데미 졸업 레벨
    public const int AcademyGraduationWarn     = 35;  // 졸업 임박 경고 레벨
}
