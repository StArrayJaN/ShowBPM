namespace ShowBPM
{
    public class Language
    {
        public string showTileBPM;
        public string showRealBPM;
        public string showKPS;

        public string setTileBPM;
        public string setRealBPM;
        public string setKPS;

        public string setShadow;
        public string setBold;
        public string setZeroPlaceHolder;
        public string showDecimal;
        public string setX;
        public string ignoreMultipress;
        public string setY;
        public string setSize;
        public string setAlign;
        public string setRealKPS;
        
        public string alignRight;
        public string alignCenter;
        public string alignLeft;
        
        public string showSpeedText;
        public string showRealKPS;
    }
    
    public class Korean : Language
    {
        public Korean()
        {
            showTileBPM = "타일 BPM 띄우기";
            showRealBPM = "체감 BPM 띄우기";
            showKPS = "체감 BPM 기준 초당 클릭 띄우기";

            setTileBPM = "타일 BPM 글자";
            setRealBPM = "체감 BPM 글자";
            setKPS = "초당 클릭 글자";
            setRealKPS = "실제 초당 클릭 글자";

            setShadow = "글자 그림자 추가";
            setBold = "글자 두껍게 만들기";
            showDecimal = "소숫점 표시 제한";
            setZeroPlaceHolder = "소숫점 0 표시";
            setX = "글자 x좌표";
            ignoreMultipress = "체감 BPM에서 동타 무시하기";
            setY = "글자 y좌표";
            setSize = "글자 크기";
            setAlign = "글자 정렬";

            alignRight = "오른쪽";
            alignCenter = "가운데";
            alignLeft = "왼쪽";
            
            showSpeedText = "속도 표시";
            showRealKPS = "실제 초당 클릭 (에디터 플레이 모드에서만 표시)";
        }
    }
    
    public class English : Language
    {
        public English()
        {
            showTileBPM = "Show Tile BPM";
            showRealBPM = "Show Real BPM";
            showKPS = "Key Per Second based on Real BPM";

            setTileBPM = "Tile BPM Text";
            setRealBPM = "Real BPM Text";
            setKPS = "Key Per Second Text";
            setRealKPS = "Real Key Per Second Text";

            setShadow = "Add Text Shadow ";
            setBold = "Set Text Bold";
            setZeroPlaceHolder = "Zero Placeholder";
            showDecimal = "Limit Decimal Display";
            setX = "Set Text X Coordinates";
            setY = "Set Text Y Coordinates";
            setSize = "Set Text Size";
            ignoreMultipress = "Ignore MultiPress at Real BPM";
            setAlign = "Set Text Align";

            alignRight = "Right";
            alignCenter = "Center";
            alignLeft = "Left";
            
            showSpeedText = "Show Speed Text";
            showRealKPS = "Real Key Per Second (Only Show in Editor Play Mode)";
        }
    }

    public class Chinese : Language
    {
        public Chinese()
        {
            showTileBPM = "显示轨道BPM";
            showRealBPM = "显示实际BPM";
            showKPS = "显示基于实际BPM的每秒按键数";

            setTileBPM = "轨道BPM文本";
            setRealBPM = "实际BPM文本";
            setKPS = "每秒按键数文本";
            setRealKPS = "每秒实际按键数文本";

            setShadow = "添加文本阴影";
            setBold = "设置文本加粗";
            setZeroPlaceHolder = "零占位符";
            showDecimal = "限制小数显示";
            setX = "设置文本X坐标";
            setY = "设置文本Y坐标";
            setSize = "设置文本大小";
            ignoreMultipress = "忽略实际BPM中的多按";
            setAlign = "设置文本对齐方式";

            alignRight = "右";
            alignCenter = "中";
            alignLeft = "左";
            
            showSpeedText = "显示速度文本";
            showRealKPS = "每秒实际按键数(仅支持编辑器下播放显示)";
        }
    }
}