public enum SFXType
{
    None = 0,

    //비버
    BeaverHit,
    BeaverRoll,
    BeaverJump,   
    //BeaverMove,

    //갈무리
    GatheringLoop,
    
    //드랍
    DropWood,
    DropStone,
    DropGlass,
    DropMetal,

    //버튼
    ButtonConfirm,
    ButtonBack,
    //ButtonMove,

    //대화
    BeaverVoice,
    SeaOtterVoice,

    //인간
    HumanChase, //배경음 음소거 필요
    HumanWalk,
    HumanAttack,
    HumanTargetFound,

    //아이템
    ItemCollect,
    ItemTrash,

    //업그레이드
    HouseUpgrade,
    StatUpgrade,

    //알림음 (튜토용)
    //내용
}
