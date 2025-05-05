import React, { useState } from "react";

interface CheckboxProps {
  label: string;
  checked?: boolean;
  onChange?: (checked: boolean) => void;
}

const Checkbox: React.FC<CheckboxProps> = ({ label, checked = false, onChange }) => {
  const [isChecked, setIsChecked] = useState(checked);

  const toggleCheckbox = () => {
    setIsChecked(!isChecked);
    if (onChange) {
      onChange(!isChecked);
    }
  };

  return (
    <div className="flex select-none self-start gap-4" onClick={toggleCheckbox}>
      <div className="flex w-6 h-6 rounded-md border-[2px] border-main-blue items-center justify-center">
        {isChecked && <div className="w-3.5 h-3.5 rounded-xs bg-main-blue" />}
      </div>
      <span className="font-semibold">{label}</span>
    </div>
  );
};

export default Checkbox;