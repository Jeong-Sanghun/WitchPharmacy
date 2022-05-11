# 홍익대학교 ExP Make팀 장기프로젝트 마녀약국
2D / 비주얼 노벨

# 플레이 영상
[프로토타입 영상](https://youtu.be/kmRkHworuqY) / [개편 영상](https://youtu.be/M3QtoRzKKFU)

# 팀구성 
**기획** : 홍영표
 
**시나리오** : 박지홍, 이다윤
 
**프로그래밍** : 정상훈

**그래픽** : 박선아, 이유진, 이가은, 강지은
 
**PM,사운드** : 이정훈

# 개발기간
2021.02 ~

# 개발툴
Unity Engine
C#

# 비주얼 노벨 시스템
## TextAsset 파싱
> RoomCounterScene(약국운영씬)에서 TextAsset을 파싱하여 사용
> 
> [StoryParser.cs](https://github.com/Jeong-Sanghun/SecuredProjects/blob/be61e1961af072c9571a38ae766cf3670039b36a/%EB%A7%88%EB%85%80%EC%95%BD%EA%B5%AD%20%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8/Scripts/ClassScript/StoryClass/StoryParser.cs) : TextAsset파싱 담당
> 
> [TextAsset예시](https://github.com/Jeong-Sanghun/SecuredProjects/blob/be61e1961af072c9571a38ae766cf3670039b36a/%EB%A7%88%EB%85%80%EC%95%BD%EA%B5%AD%20%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8/JsonData/Korean/VisitorStoryBundle/Random/RuinCity/2/0.txt)

## json스크립트
>  StoryScene에서 json스크립트 사용
>  
> [StoryDialog.cs](https://github.com/Jeong-Sanghun/SecuredProjects/blob/be61e1961af072c9571a38ae766cf3670039b36a/%EB%A7%88%EB%85%80%EC%95%BD%EA%B5%AD%20%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8/Scripts/ClassScript/StoryClass/StoryDialog.cs) : json에서의 1개의 row
