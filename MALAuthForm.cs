Copyright (C) 2020  CursedSheep

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
        
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMALAPI
{
    public partial class MALAuthForm : Form
    {
        public MALAuthForm()
        {
            InitializeComponent();
        }
        public string retKey;
        private void authorize_Click(object sender, EventArgs e)
        {
            retKey = authKey.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
