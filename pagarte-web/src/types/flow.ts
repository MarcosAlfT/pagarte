import * as FaIcons from "react-icons/fa";

/**
 * A type representing the valid string names of icons from the 'react-icons/fa' library.
 * This ensures we can't accidentally use an icon name that doesn't exist.
 */
export type FaIconName = keyof typeof FaIcons;

/**
 * Defines the structure for a single node in a navigable tree.
 * This is a recursive type, as a node can contain children of the same type.
 */
export interface FlowNode {
  id: string;
  name: string;
  levelName?: string;
  icon?: FaIconName; // Use our type-safe icon name
  isLeaf?: boolean; // True if this is a terminal node with no children
  children?: FlowNode[]; // An array of child nodes
}
