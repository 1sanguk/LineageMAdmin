using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public interface IClanService
{
    PaginatedList<Clan> Search(string? query, string? server, int? level, int page, int pageSize);
    ClanDetailViewModel? GetDetail(int id);
    Clan? GetById(int id);

    // 공지·소개
    bool UpdateNotice(int id, string notice, out Clan? clan);
    bool UpdateIntroduction(int id, string introduction, out Clan? clan);

    // 기본 정보 (레벨·명성·경험치·전적·시설 flags)
    bool UpdateBasicInfo(int id, int level, long reputation, long experience,
                         bool hasAjit, bool hasCastle, bool isAtWar,
                         int warWins, int warLosses, out Clan? clan);

    // 혈맹 설정 (가입 정책·피의 서약)
    bool UpdateSettings(int id, JoinPolicy joinPolicy, int bloodOathLevel, out Clan? clan);

    // 적대·동맹
    bool AddRival(int clanId, string rivalName);
    bool RemoveRival(int clanId, string rivalName);
    bool AddAlly(int clanId, string allyName);
    bool RemoveAlly(int clanId, string allyName);

    // 혈맹원 관리
    bool KickMember(int clanId, int characterId, out string? memberName);
    bool ChangeMemberRank(int clanId, int characterId, ClanRank newRank, out string? memberName);

    // 해산
    bool Disband(int id, out Clan? clan);
}
