21/5/19 수정사항

1. Interactable 
    - before : 버튼 클릭 시 Interactable (false)
    - after : 버튼 클릭 시 디테일메뉴 Disabled.

2. Information, Growth 활성화/비활성화 기준
    - Tag 인식 시 화면내에 다른 Tag가 접근하여도(보다 중앙에 접근해도) 기존 Tag의 Selection이 풀리지 않음.
      단 기존 Tag가 ARCamera 범위에서 벗어날 경우, UI는 기본상태로 돌아감.

3. ARCamera 범위 내에 태그가 있는 경우, menu Interactable(true)
                            없는 경우, menu Interactable(false)

4. UI 기능에 맞춰 디자인 약간 변경..

    