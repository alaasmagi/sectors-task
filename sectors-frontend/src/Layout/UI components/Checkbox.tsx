import React, { useState } from "react";
import { TouchableOpacity, Text, View, StyleSheet } from "react-native";
import { widthPercentageToDP as wp, heightPercentageToDP as hp } from "react-native-responsive-screen";

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
    <TouchableOpacity style={styles.container} onPress={toggleCheckbox}>
      <View style={[styles.checkbox]}>{isChecked && <View style={styles.innerCircle} />}</View>
      <Text style={styles.label}>{label}</Text>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: "row",
    alignItems: "center",
    gap: wp("3%"),
  },
  checkbox: {
    width: 30,
    height: 30,
    borderRadius: 8,
    borderWidth: 2,
    borderColor: "#4492EA",
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: "#1E1E1E",
  },
  innerCircle: {
    width: 17,
    height: 17,
    borderRadius: 3,
    backgroundColor: "#4492EA",
  },
  label: {
    fontSize: wp("4.5%"),
    color: "#BCBCBD",
  },
});

export default Checkbox;
