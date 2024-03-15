<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetting
    Inherits System.Windows.Forms.Form

    'Form replaces the method Disposes to clean the list of the components.  
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Creative Windows Form.
    Private components As System.ComponentModel.IContainer

    'NOTICE: The following procedure is required by the Creative Windows Form.
    'It can be modified using the Creative Windows Form.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSetting))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.grpbLocal = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rdbEasy = New System.Windows.Forms.RadioButton()
        Me.rdbHard = New System.Windows.Forms.RadioButton()
        Me.rdbLocal = New System.Windows.Forms.RadioButton()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.rdbHost = New System.Windows.Forms.RadioButton()
        Me.rdbCon = New System.Windows.Forms.RadioButton()
        Me.txtIp = New System.Windows.Forms.TextBox()
        Me.grpbRemote = New System.Windows.Forms.GroupBox()
        Me.rdbRemote = New System.Windows.Forms.RadioButton()
        Me.grpbLocal.SuspendLayout()
        Me.grpbRemote.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.Button1.Location = New System.Drawing.Point(83, 188)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 25)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "&Start"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'grpbLocal
        '
        Me.grpbLocal.Controls.Add(Me.Label1)
        Me.grpbLocal.Controls.Add(Me.rdbEasy)
        Me.grpbLocal.Controls.Add(Me.rdbHard)
        Me.grpbLocal.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.grpbLocal.Location = New System.Drawing.Point(28, 60)
        Me.grpbLocal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpbLocal.Name = "grpbLocal"
        Me.grpbLocal.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpbLocal.Size = New System.Drawing.Size(237, 110)
        Me.grpbLocal.TabIndex = 1
        Me.grpbLocal.TabStop = False
        Me.grpbLocal.Text = "Difficulty"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(184, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Select the difficulty of the opponent."
        '
        'rdbEasy
        '
        Me.rdbEasy.AutoSize = True
        Me.rdbEasy.Checked = True
        Me.rdbEasy.Location = New System.Drawing.Point(29, 64)
        Me.rdbEasy.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbEasy.Name = "rdbEasy"
        Me.rdbEasy.Size = New System.Drawing.Size(48, 17)
        Me.rdbEasy.TabIndex = 1
        Me.rdbEasy.TabStop = True
        Me.rdbEasy.Text = "Easy"
        Me.rdbEasy.UseVisualStyleBackColor = True
        '
        'rdbHard
        '
        Me.rdbHard.AutoSize = True
        Me.rdbHard.Location = New System.Drawing.Point(146, 64)
        Me.rdbHard.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbHard.Name = "rdbHard"
        Me.rdbHard.Size = New System.Drawing.Size(48, 17)
        Me.rdbHard.TabIndex = 4
        Me.rdbHard.Text = "Hard"
        Me.rdbHard.UseVisualStyleBackColor = True
        '
        'rdbLocal
        '
        Me.rdbLocal.AutoSize = True
        Me.rdbLocal.Checked = True
        Me.rdbLocal.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.rdbLocal.Location = New System.Drawing.Point(28, 22)
        Me.rdbLocal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbLocal.Name = "rdbLocal"
        Me.rdbLocal.Size = New System.Drawing.Size(154, 17)
        Me.rdbLocal.TabIndex = 5
        Me.rdbLocal.TabStop = True
        Me.rdbLocal.Text = "I want to play a local game"
        Me.rdbLocal.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(324, 50)
        Me.CheckBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(84, 19)
        Me.CheckBox1.TabIndex = 6
        Me.CheckBox1.Text = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.Button2.Location = New System.Drawing.Point(177, 188)
        Me.Button2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(87, 25)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "&Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'rdbHost
        '
        Me.rdbHost.AutoSize = True
        Me.rdbHost.Checked = True
        Me.rdbHost.Location = New System.Drawing.Point(26, 36)
        Me.rdbHost.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbHost.Name = "rdbHost"
        Me.rdbHost.Size = New System.Drawing.Size(110, 17)
        Me.rdbHost.TabIndex = 6
        Me.rdbHost.TabStop = True
        Me.rdbHost.Text = "Start a new game"
        Me.rdbHost.UseVisualStyleBackColor = True
        '
        'rdbCon
        '
        Me.rdbCon.AutoSize = True
        Me.rdbCon.Location = New System.Drawing.Point(26, 72)
        Me.rdbCon.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbCon.Name = "rdbCon"
        Me.rdbCon.Size = New System.Drawing.Size(116, 17)
        Me.rdbCon.TabIndex = 4
        Me.rdbCon.TabStop = True
        Me.rdbCon.Text = "Connect to a game"
        Me.rdbCon.UseVisualStyleBackColor = True
        '
        'txtIp
        '
        Me.txtIp.Location = New System.Drawing.Point(24, 99)
        Me.txtIp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtIp.Name = "txtIp"
        Me.txtIp.Size = New System.Drawing.Size(181, 21)
        Me.txtIp.TabIndex = 9
        '
        'grpbRemote
        '
        Me.grpbRemote.Controls.Add(Me.txtIp)
        Me.grpbRemote.Controls.Add(Me.rdbCon)
        Me.grpbRemote.Controls.Add(Me.rdbHost)
        Me.grpbRemote.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.grpbRemote.Location = New System.Drawing.Point(28, 215)
        Me.grpbRemote.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpbRemote.Name = "grpbRemote"
        Me.grpbRemote.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpbRemote.Size = New System.Drawing.Size(237, 148)
        Me.grpbRemote.TabIndex = 3
        Me.grpbRemote.TabStop = False
        Me.grpbRemote.Text = "Online play options"
        Me.grpbRemote.Visible = False
        '
        'rdbRemote
        '
        Me.rdbRemote.AutoSize = True
        Me.rdbRemote.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.rdbRemote.Location = New System.Drawing.Point(28, 188)
        Me.rdbRemote.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.rdbRemote.Name = "rdbRemote"
        Me.rdbRemote.Size = New System.Drawing.Size(198, 17)
        Me.rdbRemote.TabIndex = 6
        Me.rdbRemote.Text = "I want to play or host a game online"
        Me.rdbRemote.UseVisualStyleBackColor = True
        Me.rdbRemote.Visible = False
        '
        'frmSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 232)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.rdbRemote)
        Me.Controls.Add(Me.rdbLocal)
        Me.Controls.Add(Me.grpbRemote)
        Me.Controls.Add(Me.grpbLocal)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSetting"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "New Reversi game"
        Me.grpbLocal.ResumeLayout(False)
        Me.grpbLocal.PerformLayout()
        Me.grpbRemote.ResumeLayout(False)
        Me.grpbRemote.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents grpbLocal As System.Windows.Forms.GroupBox
    Friend WithEvents rdbLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rdbEasy As System.Windows.Forms.RadioButton
    Friend WithEvents rdbHard As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents rdbHost As System.Windows.Forms.RadioButton
    Friend WithEvents rdbCon As System.Windows.Forms.RadioButton
    Friend WithEvents txtIp As System.Windows.Forms.TextBox
    Friend WithEvents grpbRemote As System.Windows.Forms.GroupBox
    Friend WithEvents rdbRemote As System.Windows.Forms.RadioButton
End Class
