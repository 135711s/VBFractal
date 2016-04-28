Imports System.Threading
Public Class Form1

    Public thread As New Thread(AddressOf mainloop)
    Public random As New Random
    Public session As String = random.Next(1000, 10000)
    Public display As New VBGame
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
        display.setDisplay(Me, New Size(800, 600), "Fractal", True)
        thread.Start()
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
        Dim sbounds As Double = display.width + display.height
        Dim esccolort As System.Drawing.Color
        Dim esccolortHSV As New HSV

        For x = -display.width / 2 To display.width / 2

            If x Mod 50 = 0 Then
                display.update()
            End If

            If x Mod fastlevel <> 0 And fastlevel > 1 Then
                Continue For
            End If

            For y = -display.height / 2 To display.height / 2

                If y Mod fastlevel <> 0 And fastlevel > 1 Then
                    Continue For
                End If

                c.x = x / scale + shift_x
                c.y = y / scale + shift_y

                omc = c.clone()
                oc.x = x + display.width / 2
                oc.y = y + display.height / 2

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
                    display.drawRect(New Rectangle(oc.x, oc.y, fastlevel, fastlevel), esccolort)
                Else
                    display.drawRect(New Rectangle(oc.x, oc.y, fastlevel, fastlevel), VBGame.black)
                End If
            Next
        Next

    End Sub

    Sub mainloop()

        While True

            Dim zoomin As Boolean = False

            Dim b As Button
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

            b = New Button(display, "Reset")
            buttons.Add(b.clone())

            b = New Button(display, "Set Center Point...")
            buttons.Add(b.clone())

            b = New Button(display, "Toggle Fast")
            buttons.Add(b.clone())

            b = New Button(display, "Increase Iterations")
            buttons.Add(b.clone())

            b = New Button(display, "Decrease Iterations")
            buttons.Add(b.clone())

            b = New Button(display, "Random Color")
            buttons.Add(b.clone())

            b = New Button(display, "Cycle Mode")
            buttons.Add(b.clone())

            b = New Button(display, "Set C (If julia)...")
            buttons.Add(b.clone())

            For Each Button As Button In buttons
                Button.fontsize = 8
            Next

            fastlevel = maxfastlevel

            change = True

            While True

                For Each e As KeyEventArgs In display.getKeyDownEvents()

                    If e.KeyCode = Keys.R Then
                        Exit While
                    ElseIf e.KeyCode = Keys.F Then
                        change = True
                        If maxfastlevel = 1 Then
                            maxfastlevel = 5
                        Else
                            maxfastlevel = 1
                        End If
                        fastlevel = maxfastlevel
                    ElseIf e.KeyCode = Keys.Up Then
                        change = True
                        shift_y -= 256 / zoom
                    ElseIf e.KeyCode = Keys.Down Then
                        change = True
                        shift_y += 256 / zoom
                    ElseIf e.KeyCode = Keys.Left Then
                        change = True
                        shift_x -= 256 / zoom
                    ElseIf e.KeyCode = Keys.Right Then
                        change = True
                        shift_x += 256 / zoom
                    ElseIf e.KeyCode = Keys.Z Then
                        change = True
                        zoomin = Not zoomin

                    ElseIf e.KeyCode = Keys.X Then
                        change = True
                        zoom *= 2
                    ElseIf e.KeyCode = Keys.C Then
                        change = True
                        zoom /= 2

                    ElseIf e.KeyCode = Keys.N Then
                        change = True
                        iterations *= 2
                    ElseIf e.KeyCode = Keys.M Then
                        change = True
                        iterations /= 2

                    ElseIf e.KeyCode = Keys.H Then
                        change = True
                        drawhud = Not drawhud
                    End If

                Next

                For Each e As MouseEvent In display.getMouseEvents()

                    If drawhud Then

                        tx = 5
                        ty = 5

                        For Each button As Button In buttons

                            button.setRect(New Rectangle(tx, ty, Len(button.text) * 5.75, button.fontsize * 2))
                            button.setColor(VBGame.white, esccolor)
                            ty += button.fontsize * 2 + 5

                            button.draw()

                            If button.handle(e) Then
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
                    End If
                Next
                If drawhud Then

                    ty = 5

                    For Each Button In buttons
                        ty += Button.fontsize * 2 + 5
                    Next

                    If complex.mode = "random_julia" Then
                        display.drawText(New Point(tx, ty), "Formula: f(x) = z^2 + " & complex.randompoint.x & " " & complex.randompoint.y & "i", VBGame.grey, 10)
                        ty += 12
                    ElseIf complex.mode = "mandelbrot" Then
                        display.drawText(New Point(tx, ty), "Formula: f(x) = z^2 + c", VBGame.grey, 10)
                        ty += 12
                    End If

                    display.drawText(New Point(tx, ty), "Zoom factor: " & zoom, VBGame.grey, 10)
                    ty += 12
                    display.drawText(New Point(tx, ty), "Center Position: " & shift_x & " " & shift_y & "i", VBGame.grey, 10)
                    ty += 12
                    display.drawText(New Point(tx, ty), "Iterations: " & iterations, VBGame.grey, 10)
                    ty += 12
                End If

                If zoomin Then
                    zoom *= 1.2
                    fastlevel = 1
                    drawhud = False
                    change = True
                    VBGame.saveImage(display.getImageFromDisplay(), session & "_" & zoom & ".bmp")
                End If

                If zoom < 1 Then
                    zoom = 1
                End If

                If iterations < 1 Then
                    iterations = 1
                End If

                If change Then
                    display.pushMouseEvent(New MouseEvent(New Point(0, 0), MouseEvent.actions.move, MouseEvent.buttons.none))
                    If fastlevel < 1 Then
                        fastlevel = 1
                    End If
                    display.fill(Color.FromArgb(64, 255, 255, 255))
                    display.drawCenteredText(display.getRect(), "Rendering...", VBGame.black, 64)
                    display.update()
                    drawFractal()
                    display.update()
                    change = False
                End If

                display.update()
                display.clockTick(60)
            End While
        End While

    End Sub

End Class
