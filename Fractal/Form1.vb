Imports System.Threading
Public Class Form1

    Public thread As New Thread(AddressOf mainloop)
    Public random As New Random
    Public session As String = random.Next(1000, 10000)
    Public vbgame As New VBGame
    Public complex As New complex

    Dim esccolor As System.Drawing.Color = Color.FromArgb(0, 255, 255)

    Public shift_x As Double = 0
    Public shift_y As Double = 0
    Public zoom As Long = 256
    Dim iterations As Integer = 64
    Public fastlevel As Integer
    Public maxfastlevel As Integer = 5
    Public change As Boolean = False


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        vbgame.setDisplay(Me, "1280x720", "Fractal")
        'vbgame.setDisplay(Me, "1920x1080", "Fractal", True)
        thread.Start()
    End Sub

    Sub testloop()
        Dim c As New HSV(189, 51, 100)
        c.fromRGB(0, 255, 255)
        While True
            c.H += 1

            vbgame.fill(c.toRGB())

            vbgame.drawRect(New Rectangle(0, 0, 55, vbgame.height), vbgame.white)
            vbgame.drawRect(New Rectangle(0, 0, 50, vbgame.height), vbgame.black)

            vbgame.drawRect(New Rectangle(5, 0, 10, vbgame.height * (c.toRGB().R / 255)), vbgame.red)
            vbgame.drawRect(New Rectangle(20, 0, 10, vbgame.height * (c.toRGB().G / 255)), vbgame.green)
            vbgame.drawRect(New Rectangle(35, 0, 10, vbgame.height * (c.toRGB().B / 255)), vbgame.blue)

            vbgame.update()
            vbgame.clockTick(60)
        End While
    End Sub

    Sub drawFractal()

        Dim scale As Long = zoom
        Dim c As New ComplexNumber
        Dim cf As New ComplexNumber
        Dim oc As New ComplexNumber
        Dim omc As New ComplexNumber
        Dim esc As Boolean
        Dim spd As Double
        Dim i As Integer
        Dim x, y, mag As Double
        Dim sbounds As Double = vbgame.width + vbgame.height
        Dim esccolort As System.Drawing.Color
        Dim esccolortHSV As New HSV

        For x = -vbgame.width / 2 To vbgame.width / 2

            If x Mod 50 = 0 Then
                vbgame.update()
            End If

            If x Mod fastlevel <> 0 And fastlevel > 1 Then
                Continue For
            End If

            For y = -vbgame.height / 2 To vbgame.height / 2

                If y Mod fastlevel <> 0 And fastlevel > 1 Then
                    Continue For
                End If

                c.x = x / scale + shift_x
                c.y = y / scale + shift_y

                omc = c.clone()
                oc.x = x + vbgame.width / 2
                oc.y = y + vbgame.height / 2

                esc = False
                spd = 0
                'iterations = zoom / 5.12
                For i = 1 To iterations
                    spd += 1

                    c = complex.applyFormula(c, omc)

                    mag = c.getMagnitude()

                    If mag > 2 Then
                        esc = True
                    End If

                    If esc Then
                        Exit For
                    End If
                Next

                If esc Then
                    'esccolort = Color.FromArgb(esccolor.R * (spd / iterations), esccolor.G * (spd / iterations), esccolor.B * (spd / iterations))
                    esccolortHSV.fromRGB(esccolor.R, esccolor.G, esccolor.B)
                    esccolortHSV.H += (spd * 50) / 5
                    'esccolortHSV.H += Math.Log(Math.Log(mag)) / Math.Log(2)
                    'esccolortHSV.V *= (spd / iterations)
                    esccolort = esccolortHSV.toRGB()
                    vbgame.drawRect(New Rectangle(oc.x, oc.y, fastlevel, fastlevel), esccolort)
                Else
                    vbgame.drawRect(New Rectangle(oc.x, oc.y, fastlevel, fastlevel), vbgame.black)
                End If
            Next
        Next

    End Sub

    Sub mainloop()

        While True

            Dim zoomin As Boolean = False

            Dim b As New button
            Dim tx As Integer = 0
            Dim ty As Integer = 0

            Dim drawhud As Boolean = True
            Dim buttons As New List(Of button)

            esccolor = Color.FromArgb(0, 255, 255)
            complex.generateRandomPoint()
            shift_x = 0
            shift_y = 0
            zoom = 256
            iterations = 64

            b.useDisplay(vbgame)
            b.text = "Reset"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Set Center Point..."
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Toggle Fast"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Increase Iterations"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Decrease Iterations"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Random Color"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Cycle Mode"
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            b.text = "Set C (If julia)..."
            b.fontsize = 8
            b.textcolor = vbgame.black

            buttons.Add(b.clone())

            fastlevel = maxfastlevel

            change = True

            While True

                For Each e In vbgame.getKeyDownEvents()

                    If e = "R" Then
                        Exit While
                    ElseIf e = "F" Then
                        change = True
                        If maxfastlevel = 1 Then
                            maxfastlevel = 5
                        Else
                            maxfastlevel = 1
                        End If
                        fastlevel = maxfastlevel
                    ElseIf e = "Up" Then
                        change = True
                        shift_y -= 256 / zoom
                    ElseIf e = "Down" Then
                        change = True
                        shift_y += 256 / zoom
                    ElseIf e = "Left" Then
                        change = True
                        shift_x -= 256 / zoom
                    ElseIf e = "Right" Then
                        change = True
                        shift_x += 256 / zoom
                    ElseIf e = "Z" Then
                        change = True
                        If zoomin Then
                            zoomin = False
                        Else
                            zoomin = True
                        End If

                    ElseIf e = "X" Then
                        change = True
                        zoom *= 2
                    ElseIf e = "C" Then
                        change = True
                        zoom /= 2

                    ElseIf e = "N" Then
                        change = True
                        iterations *= 2
                    ElseIf e = "M" Then
                        change = True
                        iterations /= 2

                    ElseIf e = "H" Then
                        change = True
                        If drawhud Then
                            drawhud = False
                        Else
                            drawhud = True
                        End If
                    End If

                Next

                If drawhud Then
                    tx = 5
                    ty = 5
                    For Each button As button In buttons

                        button.setRect(New Rectangle(tx, ty, Len(button.text) * 5.75, button.fontsize * 2))
                        button.setColor(vbgame.white, esccolor)
                        ty += button.fontsize * 2 + 5

                        If button.handle() Then
                            change = True

                            If button.text = "Reset" Then
                                Exit While

                            ElseIf button.text = "Random Color" Then
                                esccolor = New HSV(random.Next(0, 361), 100, 100).toRGB()

                            ElseIf button.text = "Toggle Fast" Then
                                If maxfastlevel = 1 Then
                                    maxfastlevel = 5
                                Else
                                    maxfastlevel = 1
                                End If
                                fastlevel = maxfastlevel

                            ElseIf button.text = "Increase Iterations" Then
                                iterations *= 2

                            ElseIf button.text = "Decrease Iterations" Then
                                iterations /= 2

                            ElseIf button.text = "Set Center Point..." Then
                                shift_x = InputBox("Enter real number", "Set Center Point...", 0)
                                shift_y = InputBox("Enter imaginary", "Set Center Point...", 0)

                            ElseIf button.text = "Cycle Mode" Then
                                If complex.mode = "mandelbrot" Then
                                    complex.mode = "random_julia"
                                Else
                                    complex.mode = "mandelbrot"
                                End If
                            ElseIf button.text = "Set C (If julia)..." Then
                                complex.randompoint.x = InputBox("Enter real number", "Set C (If julia)...", 0)
                                complex.randompoint.y = InputBox("Enter imaginary", "Set C (If julia)...", 0)
                            End If
                        End If
                    Next

                    If complex.mode = "random_julia" Then
                        vbgame.drawText(New Point(tx, ty), "Formula: f(x) = z^2 + " & complex.randompoint.x & " " & complex.randompoint.y & "i", vbgame.grey, 10)
                        ty += 12
                    ElseIf complex.mode = "mandelbrot" Then
                        vbgame.drawText(New Point(tx, ty), "Formula: f(x) = z^2 + c", vbgame.grey, 10)
                        ty += 12
                    End If

                    vbgame.drawText(New Point(tx, ty), "Zoom factor: " & zoom, vbgame.grey, 10)
                    ty += 12
                    vbgame.drawText(New Point(tx, ty), "Center Position: " & shift_x & " " & shift_y & "i", vbgame.grey, 10)
                    ty += 12
                    vbgame.drawText(New Point(tx, ty), "Iterations: " & iterations, vbgame.grey, 10)
                    ty += 12
                End If

                If zoomin Then
                    zoom *= 1.2
                    fastlevel = 1
                    drawhud = False
                    change = True
                    vbgame.saveImage(session & "_" & zoom & ".bmp")
                End If

                If zoom < 1 Then
                    zoom = 1
                End If

                If iterations < 1 Then
                    iterations = 1
                End If

                If change Then
                    If fastlevel < 1 Then
                        fastlevel = 1
                    End If
                    vbgame.fill(Color.FromArgb(64, 255, 255, 255))
                    vbgame.drawCenteredText(vbgame.getRect(), "Rendering...", vbgame.black, 64)
                    vbgame.update()
                    drawFractal()
                    vbgame.update()
                    change = False
                End If

                vbgame.update()
                vbgame.clockTick(60)
            End While
        End While

    End Sub

End Class
