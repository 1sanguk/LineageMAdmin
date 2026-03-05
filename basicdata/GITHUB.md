# LineageMOps — GitHub 설정

## 저장소
- URL: https://github.com/1sanguk/LineageMAdmin.git
- 기본 브랜치: `main`

## 원격 설정 (git config)
```
[remote "origin"]
    url = https://github.com/1sanguk/LineageMAdmin.git
    fetch = +refs/heads/*:refs/remotes/origin/*

[branch "main"]
    remote = origin
    merge = refs/heads/main
```

## 주요 명령어
```bash
# 클론
git clone https://github.com/1sanguk/LineageMAdmin.git

# 푸시
git add .
git commit -m "커밋 메시지"
git push origin main

# 풀
git pull origin main
```
