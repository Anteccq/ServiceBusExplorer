#region Copyright
//=======================================================================================
// Microsoft Azure Customer Advisory Team 
//
// This sample is supplemental to the technical guidance published on my personal
// blog at http://blogs.msdn.com/b/paolos/. 
// 
// Author: Paolo Salvatori
//=======================================================================================
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// LICENSED UNDER THE APACHE LICENSE, VERSION 2.0 (THE "LICENSE"); YOU MAY NOT USE THESE 
// FILES EXCEPT IN COMPLIANCE WITH THE LICENSE. YOU MAY OBTAIN A COPY OF THE LICENSE AT 
// http://www.apache.org/licenses/LICENSE-2.0
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE DISTRIBUTED UNDER THE 
// LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY 
// KIND, EITHER EXPRESS OR IMPLIED. SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING 
// PERMISSIONS AND LIMITATIONS UNDER THE LICENSE.
//=======================================================================================
#endregion

#region Using Directives

using System.Linq;
using System.Windows.Forms;

#endregion

namespace Microsoft.Azure.ServiceBusExplorer.Helpers
{
    public class TreeNodeExpandedState
    {
        readonly string name;
        readonly bool isExpanded;
        readonly TreeNodeExpandedState[] children;

        TreeNodeExpandedState(string name, bool isExpanded, TreeNodeExpandedState[] children)
        {
            this.name = name;
            this.isExpanded = isExpanded;
            this.children = children;
        }

        public static TreeNodeExpandedState Save(TreeView treeView)
        {
            return SaveCore(null, true, treeView.Nodes);
        }

        public static TreeNodeExpandedState Save(TreeNode node)
        {
            return SaveCore(null, node.IsExpanded, node.Nodes);
        }

        static TreeNodeExpandedState SaveCore(string name, bool isExpanded, TreeNodeCollection nodes)
        {
            return new TreeNodeExpandedState(name, isExpanded,
                nodes.Cast<TreeNode>().Where(n => n.IsExpanded).Select(n => SaveCore(n.Name, n.IsExpanded, n.Nodes)).ToArray());
        }

        public void Restore(TreeView treeView)
        {
            RestoreCore(this, treeView.Nodes);
        }

        public void Restore(TreeNode node)
        {
            if (isExpanded)
            {
                node.Expand();
            }
            RestoreCore(this, node.Nodes);
        }

        static void RestoreCore(TreeNodeExpandedState state, TreeNodeCollection nodes)
        {
            foreach (var child in state.children)
            {
                var node = nodes[child.name];
                if (node != null)
                {
                    node.Expand();
                    RestoreCore(child, node.Nodes);
                }
            }
        }
    }
}
