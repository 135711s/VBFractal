Public Class HSV

    Public H As Double = 0 '0 - 360
    Public S As Double = 0 '0 - 100
    Public V As Double = 0 '0 - 100

    Public Sub New(Optional ht As Double = 0, Optional st As Double = 0, Optional vt As Double = 0)
        H = ht
        S = st
        V = vt
    End Sub

    Sub fromRGB(r As Double, g As Double, b As Double)
        Dim c As System.Drawing.Color = Color.FromArgb(r, g, b)
        Dim max As Double = Math.Max(c.R, Math.Max(c.G, c.B))
        Dim min As Double = Math.Min(c.R, Math.Min(c.G, c.B))
        H = c.GetHue()
        S = (1 - (1 * min / max)) * 100
        If max = 0 Then S = 0
        V = (max / 255) * 100
    End Sub

    Function toRGB() As System.Drawing.Color
        Dim c, m, r, g, b, x, h2, v2, s2 As Double

        If V = 0 Then
            Return Color.Black
        End If

        While H > 360
            H -= 360
        End While
        While H < 0
            H += 360
        End While

        If V < 0 Then
            V = 0
        ElseIf V > 100 Then
            V = 100
        End If

        If S < 0 Then
            S = 0
        ElseIf S > 100 Then
            S = 100
        End If

        'S = 100 - S
        s2 = S / 100

        'V /= 2
        'V += 50
        v2 = V / 100

        h2 = H / 60

        'c = Math.Abs(1 - Math.Abs(2 * (V - 1)))
        c = v2 * s2

        'x = (1 - Math.Abs(h2 Mod 2 - 1)) * c
        x = c * (1 - Math.Abs((h2 Mod 2) - 1))
        m = v2 - c

        If h2 < 1 Then
            r = c
            g = x
            b = 0
        ElseIf h2 < 2 Then
            r = x
            g = c
            b = 0
        ElseIf h2 < 3 Then
            r = 0
            g = c
            b = x
        ElseIf h2 < 4 Then
            r = 0
            g = x
            b = c
        ElseIf h2 < 5 Then
            r = x
            g = 0
            b = c
        ElseIf h2 <= 6 Then
            r = c
            g = 0
            b = x
        End If
        r = (r + m) * 255
        g = (g + m) * 255
        b = (b + m) * 255

        'satpoint = (r + g + b) / 3

        'If r >= g And r >= b Then
        '    satpoint = r
        'ElseIf g >= r And g >= b Then
        '    satpoint = g
        'ElseIf b >= r And b >= g Then
        '    satpoint = b
        'End If

        'r = ((r - satpoint) * S) + satpoint
        'g = ((g - satpoint) * S) + satpoint
        'b = ((b - satpoint) * S) + satpoint

        'V *= 100
        'V -= 50
        'V *= 2

        'S *= 100
        'S = 100 - S

        'Return Color.FromArgb(r * 255, g * 255, b * 255)
        Return Color.FromArgb(r, g, b)
    End Function

End Class
