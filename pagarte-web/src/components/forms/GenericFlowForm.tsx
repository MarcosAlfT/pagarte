import React, { useState } from "react";
import * as FaIcons from "react-icons/fa";
import { FaArrowLeft } from "react-icons/fa";

// 1. Import the RAW JSON data
import flowDataJson from "../../data/flow.json";

// 2. Import the TYPE DEFINITION and helper types
import type { FlowNode, FaIconName } from "../../types/flow";

import GenericButton from "../controls/GenericButton";

// Type-safe helper to get an icon component by its string name
function getIcon(iconName?: FaIconName, className = "text-3xl") {
  if (!iconName) return null;
  const Icon = FaIcons[iconName];
  return Icon ? <Icon className={className} /> : null;
}

const BACK_BUTTON_WIDTH = "w-[120px]";

const GenericFlowForm: React.FC = () => {
  // 3. Perform a TYPE ASSERTION at the top level.
  // This tells TypeScript: "I guarantee that the `flowDataJson` I imported
  // has the exact shape of the `FlowNode` interface."
  // This is the correct place for this assertion.
  const rootNode: FlowNode = flowDataJson as FlowNode;

  // State to track the user's navigation path through the tree
  const [path, setPath] = useState<FlowNode[]>([]);

  // The current node to display is the last one in the path, or the root if the path is empty
  const currentNode = path.length === 0 ? rootNode : path[path.length - 1];
  const atRoot = path.length === 0;

  // Navigation handlers
  const handleSelect = (node: FlowNode) => setPath([...path, node]);
  const handleBack = () => setPath(path.slice(0, -1));
  const handleLeafAction = (leafNode: FlowNode) => {
    alert(`Action for: ${leafNode.name}`);
  };

  return (
    <div className="w-full">
      {/* Header with Back Button and Title */}
      <div className="flex items-center mb-6 min-h-[48px]">
        <div className={BACK_BUTTON_WIDTH}>
          {!atRoot && (
            <GenericButton
              type="button"
              variant="outline"
              onClick={handleBack}
              icon={<FaArrowLeft />}
              text="Back"
            />
          )}
        </div>
        <h2 className="flex-1 text-xl font-bold text-primary text-center flex items-center justify-center gap-3">
          {getIcon(currentNode.icon)}
          {/* Use the name from the root node when at the top level */}
          <span>{atRoot ? "Make a Payment" : currentNode.name}</span>
        </h2>
        <div className={BACK_BUTTON_WIDTH} />
      </div>

      {/* Grid of options */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {currentNode.children?.map((child) => (
          <GenericButton
            key={child.id}
            type="button"
            variant="primary"
            className="flex flex-col items-center justify-center h-28 sm:h-32 w-full text-base font-semibold rounded-lg py-4 px-2"
            onClick={() =>
              child.isLeaf ? handleLeafAction(child) : handleSelect(child)
            }
            icon={getIcon(child.icon, "text-4xl mb-2")}
          >
            <span className="text-center">{child.name}</span>
          </GenericButton>
        ))}
      </div>
    </div>
  );
};

export default GenericFlowForm;
