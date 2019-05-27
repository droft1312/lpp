namespace LPP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.processInput = new System.Windows.Forms.Button();
            this.graphPanel = new System.Windows.Forms.Panel();
            this.graphPicture = new System.Windows.Forms.PictureBox();
            this.outputTextbox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propositionsNamesButton = new System.Windows.Forms.Button();
            this.truthtableButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.infixTextBox = new System.Windows.Forms.TextBox();
            this.simplifyTruthTableButton = new System.Windows.Forms.Button();
            this.disjunctiveFormButton = new System.Windows.Forms.Button();
            this.nandifyButton = new System.Windows.Forms.Button();
            this.graphPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.graphPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(14, 14);
            this.inputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(226, 23);
            this.inputTextBox.TabIndex = 0;
            this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextBox_KeyDown);
            // 
            // processInput
            // 
            this.processInput.Location = new System.Drawing.Point(14, 44);
            this.processInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.processInput.Name = "processInput";
            this.processInput.Size = new System.Drawing.Size(88, 27);
            this.processInput.TabIndex = 1;
            this.processInput.Text = "Process";
            this.processInput.UseVisualStyleBackColor = true;
            this.processInput.Click += new System.EventHandler(this.processInput_Click);
            // 
            // graphPanel
            // 
            this.graphPanel.AutoScroll = true;
            this.graphPanel.Controls.Add(this.graphPicture);
            this.graphPanel.Location = new System.Drawing.Point(14, 77);
            this.graphPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(468, 428);
            this.graphPanel.TabIndex = 2;
            // 
            // graphPicture
            // 
            this.graphPicture.Image = ((System.Drawing.Image) (resources.GetObject("graphPicture.Image")));
            this.graphPicture.InitialImage = null;
            this.graphPicture.Location = new System.Drawing.Point(4, 3);
            this.graphPicture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.graphPicture.Name = "graphPicture";
            this.graphPicture.Size = new System.Drawing.Size(1024, 683);
            this.graphPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.graphPicture.TabIndex = 0;
            this.graphPicture.TabStop = false;
            // 
            // outputTextbox
            // 
            this.outputTextbox.Location = new System.Drawing.Point(489, 105);
            this.outputTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.Size = new System.Drawing.Size(430, 214);
            this.outputTextbox.TabIndex = 3;
            this.outputTextbox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(870, 77);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Output:";
            // 
            // propositionsNamesButton
            // 
            this.propositionsNamesButton.Location = new System.Drawing.Point(832, 10);
            this.propositionsNamesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propositionsNamesButton.Name = "propositionsNamesButton";
            this.propositionsNamesButton.Size = new System.Drawing.Size(88, 27);
            this.propositionsNamesButton.TabIndex = 5;
            this.propositionsNamesButton.Text = "Get Props";
            this.propositionsNamesButton.UseVisualStyleBackColor = true;
            this.propositionsNamesButton.Click += new System.EventHandler(this.propositionsNamesButton_Click);
            // 
            // truthtableButton
            // 
            this.truthtableButton.Location = new System.Drawing.Point(737, 10);
            this.truthtableButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.truthtableButton.Name = "truthtableButton";
            this.truthtableButton.Size = new System.Drawing.Size(88, 27);
            this.truthtableButton.TabIndex = 6;
            this.truthtableButton.Text = "Truth-Table";
            this.truthtableButton.UseVisualStyleBackColor = true;
            this.truthtableButton.Click += new System.EventHandler(this.truthtableButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label2.Location = new System.Drawing.Point(267, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Infix:";
            // 
            // infixTextBox
            // 
            this.infixTextBox.Location = new System.Drawing.Point(313, 14);
            this.infixTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.infixTextBox.Name = "infixTextBox";
            this.infixTextBox.Size = new System.Drawing.Size(303, 23);
            this.infixTextBox.TabIndex = 8;
            // 
            // simplifyTruthTableButton
            // 
            this.simplifyTruthTableButton.Location = new System.Drawing.Point(737, 44);
            this.simplifyTruthTableButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.simplifyTruthTableButton.Name = "simplifyTruthTableButton";
            this.simplifyTruthTableButton.Size = new System.Drawing.Size(88, 27);
            this.simplifyTruthTableButton.TabIndex = 9;
            this.simplifyTruthTableButton.Text = "Simplify";
            this.simplifyTruthTableButton.UseVisualStyleBackColor = true;
            this.simplifyTruthTableButton.Click += new System.EventHandler(this.simplifyTruthTableButton_Click);
            // 
            // disjunctiveFormButton
            // 
            this.disjunctiveFormButton.Location = new System.Drawing.Point(832, 44);
            this.disjunctiveFormButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.disjunctiveFormButton.Name = "disjunctiveFormButton";
            this.disjunctiveFormButton.Size = new System.Drawing.Size(88, 27);
            this.disjunctiveFormButton.TabIndex = 10;
            this.disjunctiveFormButton.Text = "Disjunctive";
            this.disjunctiveFormButton.UseVisualStyleBackColor = true;
            this.disjunctiveFormButton.Click += new System.EventHandler(this.disjunctiveFormButton_Click);
            // 
            // nandifyButton
            // 
            this.nandifyButton.Location = new System.Drawing.Point(655, 48);
            this.nandifyButton.Name = "nandifyButton";
            this.nandifyButton.Size = new System.Drawing.Size(75, 23);
            this.nandifyButton.TabIndex = 11;
            this.nandifyButton.Text = "Nandify";
            this.nandifyButton.UseVisualStyleBackColor = true;
            this.nandifyButton.Click += new System.EventHandler(this.nandifyButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.nandifyButton);
            this.Controls.Add(this.disjunctiveFormButton);
            this.Controls.Add(this.simplifyTruthTableButton);
            this.Controls.Add(this.infixTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.truthtableButton);
            this.Controls.Add(this.propositionsNamesButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputTextbox);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.processInput);
            this.Controls.Add(this.inputTextBox);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.graphPanel.ResumeLayout(false);
            this.graphPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.graphPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button processInput;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.PictureBox graphPicture;
        private System.Windows.Forms.RichTextBox outputTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button propositionsNamesButton;
        private System.Windows.Forms.Button truthtableButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox infixTextBox;
        private System.Windows.Forms.Button simplifyTruthTableButton;
        private System.Windows.Forms.Button nandifyButton;
        private System.Windows.Forms.Button disjunctiveFormButton;
    }
}

