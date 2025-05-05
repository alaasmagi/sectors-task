import { Icons } from "./Icons";

interface DropDownListOption {
  value: string;
  label: string;
  indentLevel?: number;
}

interface DropDownListProperties {
  icon: keyof typeof Icons;
  options: DropDownListOption[];
  onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  label: string;
  value?: string;
}

const DropDownList: React.FC<DropDownListProperties> = ({ icon, options, value, label, onChange }) => {
  return (
    <div className="flex flex-col w-full">
      <div className="flex flex-row w-max-full items-center gap-1.5">
        <img src={Icons[icon]} className="h-7" />
        <select
          value={value}
          onChange={onChange}
          className="outline-none text-main-text w-full text-lg bg-main-dark"
        >
          <option key="" value="">
            {label}
          </option>
          {options.map((option) => (
            <option
              key={option.value}
              value={option.value}
              style={{ paddingLeft: `${(option.indentLevel ?? 0) * 1.5}rem` }}
            >
              {option.label}
            </option>
          ))}
        </select>
      </div>
      <div className="w-full h-px bg-main-text mt-2"></div>
    </div>
  );
};

export default DropDownList;
