# PathFind_Tutorial
## 프로젝트 소개
해당 프로젝트는 A* 알고리즘과 Jump Point Search 알고리즘을 보다 쉽게 학습하도록 하기 위해 만든 프로젝트입니다.
제한된 공간에 이동 가능 위치, 불가능 위치를 지정하여 길을 표시해주는 기능을 합니다.

## 사용법
PathFinder_JPS, PathFinder 오브젝트의 btn_x, btn_y 값을 입력해주면 각각 Jump Point Search, A* 알고리즘을 통해 원점에서 해당 위치까지의 경로를 나타냅니다.
보다 쉽게 확인하기 위해 각 노드를 오브젝트로써 두어 가시적으로 확인이 용이하게 하였습니다.

## 코드 설명
GridCreator는 Room Size에 따라 해당 크기만큼의 공간을 1x1 사이즈의 사각형의 노드들로 구성합니다.
Node는 각각 노드가 갖는 가중치와 이웃한 노드들에 대한 정보들을 갖습니다.
PathFinder는 A* 알고리즘을 구현한 코드입니다.
PathFinder_JPS는 Jump Point Search를 구현한 코드입니다.

## 개선해야할 사항
1. Node를 오브젝트로 안 두고 사용하기
2. layer 기준으로 노드의 이동 가능 여부 판단하기
3. 코드 간소화, 최적화
4. 원리에 대한 추가 설명 필요
