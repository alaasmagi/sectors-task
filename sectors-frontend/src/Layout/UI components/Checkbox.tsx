import React from "react";

interface CheckboxProps {
  label: string;
  checked: boolean;
  onChange: (checked: boolean) => void;
}

const Checkbox: React.FC<CheckboxProps> = ({ label, checked, onChange }) => {
  const toggleCheckbox = () => {
    onChange(!checked);
  };

  return (
    <div className="flex select-none self-start gap-4 cursor-pointer" onClick={toggleCheckbox}>
      <div className="flex w-6 h-6 rounded-md border-[2px] border-main-blue items-center justify-center">
        {checked && <div className="w-3.5 h-3.5 rounded-xs bg-main-blue" />}
      </div>
      <span className="font-semibold">{label}</span>
    </div>
  );
};

export default Checkbox;
