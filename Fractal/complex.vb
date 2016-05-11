Public Class Complex

    Private random = New Random
    Public juliapoint As ComplexNumber = New ComplexNumber
    Public mode As Byte = modes.mandelbrot

    Enum modes
        mandelbrot
        julia
    End Enum

    Function generateFractal(size As Size, pixelSkip As Integer, scale As Long, shift_x As Double, shift_y As Double, iterations As Integer, basecolor As System.Drawing.Color) As Bitmap

        Dim display As New BitmapSurface(size)

        Dim pixel As New ComplexNumber
        Dim displayPixel As New ComplexNumber
        Dim OriginalPixel As New ComplexNumber
        Dim escaped As Boolean
        Dim escapeSpeed As Double
        Dim i As Integer
        Dim x, y, magnitude As Double
        Dim esccolort As System.Drawing.Color
        Dim esccolortHSV As New HSV

        For x = -display.width / 2 To display.width / 2

            If x Mod pixelSkip <> 0 Then
                Continue For
            End If

            For y = -display.height / 2 To display.height / 2

                If y Mod pixelSkip <> 0 Then
                    Continue For
                End If

                pixel.x = x / scale + shift_x
                pixel.y = y / scale + shift_y

                OriginalPixel = pixel.clone()
                displayPixel.x = x + display.width / 2
                displayPixel.y = y + display.height / 2

                escaped = False
                escapeSpeed = 0
                'iterations = zoom / 5.12
                For i = 1 To iterations
                    escapeSpeed += 1

                    pixel = applyFormula(pixel, OriginalPixel)

                    magnitude = pixel.getMagnitude()

                    If magnitude > 2 Then
                        escaped = True
                    End If

                    If escaped Then
                        Exit For
                    End If
                Next

                If escaped Then

                    esccolortHSV = HSV.fromRGB(basecolor.R, basecolor.G, basecolor.B)
                    esccolortHSV.H += (escapeSpeed * 50) / 5
                    'esccolortHSV.H = 360 * ((escapeSpeed + 1 - Math.Log(Math.Log(Math.Abs(magnitude))) / Math.Log(2)) / iterations)
                    esccolort = esccolortHSV.toRGB()

                    display.drawRect(New Rectangle(displayPixel.x, displayPixel.y, pixelSkip, pixelSkip), esccolort)
                Else
                    display.drawRect(New Rectangle(displayPixel.x, displayPixel.y, pixelSkip, pixelSkip), VBGame.black)
                End If
            Next
        Next
        Return display.getImage()
    End Function

    Sub generateRandomPoint()
        juliapoint.x = random.next(-1000000, 1000001) / 1000000
        juliapoint.y = random.next(-1000000, 1000001) / 1000000
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
        'cf = add(multiply(c, c), New ComplexNumber(0.324311999999999, -0.0496199999999995)) 'swirly thing

        If mode = modes.mandelbrot Then
            cf = add(multiply(c, c), oc)
        ElseIf mode = modes.julia Then
            cf = add(multiply(c, c), juliapoint)
        End If

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
