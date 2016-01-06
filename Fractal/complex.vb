Public Class complex

    Private random = New Random
    Public randompoint As ComplexNumber = New ComplexNumber
    Public mode = "mandelbrot"

    Sub generateRandomPoint()
        randompoint.x = random.next(-1000000, 1000001) / 1000000
        randompoint.y = random.next(-1000000, 1000001) / 1000000
    End Sub

    Function add(c1 As ComplexNumber, c2 As ComplexNumber) As ComplexNumber
        Dim cf As New ComplexNumber
        cf.x = c1.x + c2.x
        cf.y = c1.y + c2.y
        Return cf
    End Function

    Function subtract(c1 As ComplexNumber, c2 As ComplexNumber) As ComplexNumber
        Dim cf As New ComplexNumber
        cf.x = c1.x - c2.x
        cf.y = c1.y - c2.y
        Return cf
    End Function

    Function multiply(c1 As ComplexNumber, c2 As ComplexNumber) As ComplexNumber
        Dim cf As New ComplexNumber
        cf.x = (c1.x * c2.x) - (c1.y * c2.y)
        cf.y = (c1.x * c2.y) + (c1.y * c2.x)
        Return cf
    End Function

    Function applyFormula(c As ComplexNumber, oc As ComplexNumber) As ComplexNumber
        Dim cf As New ComplexNumber
        'cf = multiply(c, c) 'circle
        'cf = add(multiply(c, c), oc) 'mandlebrot
        'cf = add(multiply(c, c), New ComplexNumber(-0.1, 0.7)) 'rabbit
        'cf = add(multiply(c, c), New ComplexNumber(-0.835, 0.2321)) 'dragon
        'cf = add(multiply(c, c), New ComplexNumber(-0.8, 0.156)) 'swirly dragon
        'cf = add(multiply(c, c), randompoint)
        If mode = "mandelbrot" Then
            cf = add(multiply(c, c), oc)
        ElseIf mode = "random_julia" Then
            cf = add(multiply(c, c), randompoint)
        End If
        'cf = New ComplexNumber(12, 12)
        Return cf
    End Function
End Class

Public Class ComplexNumber

    Public x As Double
    Public y As Double

    Public Sub New(Optional xt As Double = 0, Optional yt As Double = 0)
        x = xt
        y = yt
    End Sub

    Public Function clone() As ComplexNumber
        Return DirectCast(Me.MemberwiseClone(), ComplexNumber)
    End Function

    Function getMagnitude() As Double
        Return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))
    End Function

    Function getPoint() As Point
        Return New Point(x, y)
    End Function

    Function getPointF() As PointF
        Return New PointF(x, y)
    End Function

End Class