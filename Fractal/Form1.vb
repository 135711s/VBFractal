Imports System.Threading
Public Class Form1

    Public thread As New Thread(AddressOf mainloop)
    Public random As New Random
    Public session As String = random.Next(1000, 10000)
    Public display As New VBGame
    Public complex As New complex

    Dim color As System.Drawing.Color = color.FromArgb(0, 255, 255)

    Public imageSize As Integer = 800
    Public pixelSkip As Integer = 5
    Public shift_x As Double = 0
    Public shift_y As Double = 0
    Public zoom As Long = 256
    Dim iterations As Integer = 64

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        display.setDisplay(Me, New Size(800, 800), "Fractal", True)
        thread.Start()
    End Sub

    Sub mainloop()

        While True

            For Each e As KeyEventArgs In display.getKeyUpEvents()
                If e.KeyCode = Keys.Z Then
                    zoom *= 2
                ElseIf e.KeyCode = Keys.X Then
                    zoom /= 2
                ElseIf e.KeyCode.ToString().Contains("D") And e.KeyCode <> Keys.D Then
                    pixelSkip = e.KeyCode.ToString().Replace("D", "")

                ElseIf e.KeyCode = Keys.Left Then
                    shift_x -= 1
                ElseIf e.KeyCode = Keys.Right Then
                    shift_x += 1
                ElseIf e.KeyCode = Keys.Up Then
                    shift_y -= 1
                ElseIf e.KeyCode = Keys.Down Then
                    shift_y += 1
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
            display.update()
        End While
    End Sub

End Class
