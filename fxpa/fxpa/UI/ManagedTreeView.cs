// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Windows.Forms;
using System.Drawing;

namespace fxpa
{
	/// <summary>
	/// Summary description for ManagedTree.
	/// </summary>
	public class ManagedTreeView : TreeView
	{
		public ManagedTreeView()
		{
		}

		IManagedTreeNode _root;
		public IManagedTreeNode Root
		{
			get
			{
				return _root;
			}
			set
			{
				_root = value;
				CreateTree();
			}
		}

		protected void CreateTree()
		{
			this.Nodes.Clear();
			if ( _root != null )
			{
				_root.TreeModifiedEvent += new ModifiedDelegate(NodeModified);
				TreeNode rootNode = new TreeNode();
				UpdateNode(rootNode, _root);
				this.Nodes.Add(rootNode);
			}
			this.ExpandAll();
		}

		public TreeNode GetNodeByTag(TreeNodeCollection nodes, object tag)
		{
			foreach(TreeNode node in nodes)
			{
				if ( node.Tag == tag )
				{
					return node;
				}
				if ( GetNodeByTag(node.Nodes, tag) != null )
				{
					return GetNodeByTag(node.Nodes, tag);
				}
			}
			return null;
		}

		public void NodeModified(IManagedTreeNode node)
		{
			this.SuspendLayout();

			TreeNode treeNode = GetNodeByTag(Nodes, node);
			if ( treeNode != null )
			{
				UpdateNode(treeNode, node);
			}

			this.ExpandAll();
			this.ResumeLayout();
		}

		protected void UpdateNode(TreeNode treeNode, IManagedTreeNode node)
		{
			treeNode.Tag = node;
			treeNode.Text = node.TreeText;
			treeNode.ImageIndex = node.TreeImageIndex;
			treeNode.SelectedImageIndex = node.TreeImageIndex;

			for(int i=0; i<node.TreeChildren.Length; i++)
			{
				if ( treeNode.Nodes.Count > i )
				{// Update child.
					UpdateNode(treeNode.Nodes[i], node.TreeChildren[i]);
					continue;
				}
				
				// New child.
				TreeNode newChild = new TreeNode();
				node.TreeChildren[i].TreeModifiedEvent += new ModifiedDelegate(NodeModified);
				UpdateNode(newChild, node.TreeChildren[i]);
				treeNode.Nodes.Add(newChild);
			}

			for(int i=treeNode.Nodes.Count-1; i>=node.TreeChildren.Length; i--)
			{
				treeNode.Nodes[i].Remove();
			}
		}
    }

    public class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawSubItem(e);
            double d;

            if (e.Item.BackColor != Color.MediumPurple)
            {
                if (e.Item.Index % 2 == 0) //theSubItemIndex是你指定要重绘的列
                {
                    //e.Item.BackColor = Color.FromArgb(247, 247, 247);

                    e.Item.BackColor = Color.Azure;
                    int count = e.Item.SubItems.Count - 1;
                    while (count >= 1)
                    {
                        e.Item.SubItems[count].BackColor = Color.Azure;
                        count--;
                    }
                    //e.Item.SubItems[2].BackColor = Color.FromArgb(247, 247, 247);
                }
                else
                {
                    //e.Item.ForeColor = Color.;
                }
            }
        }
    }
}
