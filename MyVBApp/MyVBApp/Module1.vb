Module Module1

    Sub Main()
        'If Format.NamedSubStrings("M_Country").Value = "11111" Then "★" else"♥" end if

        Dim Value = "11111"
        'If Value.Chars(0) = "1" Or Value.Chars(0) = "2" Or Value.Chars(0) = "3" Or Value.Chars(0) = "4" Then Value = "★" Else Value = "♥"

        If Value.StartsWith("1") Then Value = "★" Else Value = "♥"
        Console.WriteLine(Value)
        'If Left(Value, 1) = "1" or Left(Value, 1) = "2" or Left(Value, 1) = "3" or Left(Value, 1) = "4" Then Value = "★" Else Value = "♥" End If
    End Sub

End Module
