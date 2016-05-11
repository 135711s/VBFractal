Imports System.Threading

Public Class Form1

    Public thread As New Thread(AddressOf mainloop)
    Public random As New Random
    Public session As String = random.Next(1000, 10000)
    Public display As New VBGame
    Public complex As New Complex

    Dim color As System.Drawing.Color = color.FromArgb(0, 255, 255)

    Public imageSize As Integer = 600
    Public pixelSkip As Integer = 5
    Public shift_x As Double = 0
    Public shift_y As Double = 0
    Public zoom As Long = 256
    Dim iterations As Integer = 64

    Public Sub resetValues()
        shift_x = 0
        shift_y = 0
        zoom = 256
        iterations = 64
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        display.setDisplay(Me, New Size(imageSize, imageSize), "Fractal", True)
        thread.Start()
    End Sub

    Sub mainloop()

        complex.mode = complex.modes.julia
        complex.generateRandomPoint()

        Dim modifier As Double = 0.01
        Dim frame As Integer = 1

        While True

            For Each e As KeyEventArgs In display.getKeyUpEvents()
                If e.KeyCode = Keys.Z Then
                    zoom *= 2
                ElseIf e.KeyCode = Keys.X Then
                    zoom /= 2
                ElseIf e.KeyCode.ToString().Contains("D") And e.KeyCode <> Keys.D And e.KeyCode <> Keys.Down Then
                    pixelSkip = e.KeyCode.ToString().Replace("D", "")

                ElseIf e.KeyCode = Keys.Left Then
                    shift_x -= 256 / zoom
                ElseIf e.KeyCode = Keys.Right Then
                    shift_x += 256 / zoom
                ElseIf e.KeyCode = Keys.Up Then
                    shift_y -= 256 / zoom
                ElseIf e.KeyCode = Keys.Down Then
                    shift_y += 256 / zoom

                ElseIf e.KeyCode = Keys.I Then
                    iterations *= 2
                ElseIf e.KeyCode = Keys.O Then
                    iterations /= 2

                ElseIf e.KeyCode = Keys.R Then
                    resetValues()
                    complex.generateRandomPoint()

                ElseIf e.KeyCode = Keys.ControlKey Then
                    If complex.juliapoint.x < 0 Then
                        complex.juliapoint = New ComplexNumber(complex.juliapoint.x + modifier, complex.juliapoint.y)
                    Else
                        complex.juliapoint = New ComplexNumber(complex.juliapoint.x - modifier, complex.juliapoint.y)
                    End If
                    If complex.juliapoint.y < 0 Then
                        complex.juliapoint = New ComplexNumber(complex.juliapoint.x, complex.juliapoint.y + modifier)
                    Else
                        complex.juliapoint = New ComplexNumber(complex.juliapoint.x, complex.juliapoint.y - modifier)
                    End If

                ElseIf e.KeyCode = Keys.M Then
                    complex.mode += 1
                    If complex.mode > 1 Then
                        complex.mode = 0
                    End If
                End If
            Next

            For Each e As MouseEvent In display.getMouseEvents()
                If e.action = MouseEvent.actions.scroll Then
                    If e.button = MouseEvent.buttons.scrollUp Then
                        zoom *= 2
                    ElseIf e.button = MouseEvent.buttons.scrollDown Then
                        zoom /= 2
                    End If
                End If
            Next

            display.blit(complex.generateFractal(New Size(imageSize, imageSize), pixelSkip, zoom, shift_x, shift_y, iterations, color), display.getRect())
            'VBGame.saveImage(display.getImageFromDisplay(), session & "_" & frame & "_" & complex.juliapoint.x & "_" & complex.juliapoint.y & ".png")
            frame += 1
            display.update()
        End While
    End Sub

End Class
