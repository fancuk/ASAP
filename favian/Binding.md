제가 Binding을 하면서 알게된 Binding 클래스의 UpdateSourceTrigger입니다!<br>

UpdateSourceTrigger에 대해서 
짧게 설명하자면 Text,Content 등등 Source가 바뀌게 될때, 언제 Binding Target이 바뀌는지 입니다!

## 1. UpdateSourceTrigger = PropertyChanged

이 PropertyChanged는 바인딩 대상 속성이 변경될 때마다
바인딩 소스를 즉시 업데이트 해 줍니다.

> 만약 TextBox에 123을 입력하면, 1 -> 12 -> 123 이런식으로 타자를 칠 때마다 바뀐다는 뜻 입니다 ㅎ

## 2. UpdateSourceTrigger = LostFocus

이 LostFocus는 말 그대로 Focus를 잃었을때 
바인딩 소스가 업데이트 됩니다.

> TextBox에 123을 입력 후, 다른 Button, 다른 TextBox 클릭하는 순간!! 바뀐다는 뜻 입니다 ㅎ

## 3. UpdateSourceTrigger = Explicit

이 Explicit는 UpdateSource 메서드를 호출할 때만 바인딩 소스를 업데이트 해줍니다 ㅎ

> BindingExpression be = textBox1.GetBindingExpression(TextBox,TextProperty); <br>
be.UpdateSource(); 이런 식입니다 ㅎ

저도 아직 한번도 안써봐서 Explicit는 자세히 잘 모르겠습니다 ㅠ

## 4. Default

그냥 바인딩을 하면, 대부분의 경우는 PropertyChanged로 되지만,
Text의 경우는 LostFocus 입니다.

저는 개인적으로 LostFocus를 선호하는 편 입니다.
왜냐하면 PropertyChanged는 시간이 LostFocus보다는 많이 걸리기 때문입니다.

이상으로 소소한 팁 마치겠습니다. 읽어주셔서 감사합니다 ㅎ
