import { useEffect, useState } from "react";
import "../../App.css";
import { useNavigate, useParams } from "react-router-dom";
import PersonModel from "../../Models/PersonModel";
import SectorModel from "../../Models/SectorModel";
import { AddPerson, GetAllSectors, GetPersonById, RemovePerson, UpdatePerson } from "../../BusinessLogic/PersonData";
import TextBox from "../UI components/TextBox";
import DropDownList from "../UI components/DropdownList";
import Checkbox from "../UI components/Checkbox";
import NormalButton from "../UI components/NormalButton";
import DropdownModel from "../../Models/DropdownModel";
import NormalLink from "../UI components/Link";
import ErrorMessage from "../UI components/ErrorMessage";
import SuccessMessage from "../UI components/SuccessMessage";

function EditView() {
  const { id } = useParams();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [currentSessionData, setCurrentSessionData] = useState<PersonModel | null>(null);
  const [selectedSectorId, setSelectedSectorId] = useState<string>("");
  const [insertedFullName, setInsertedFullName] = useState<string>("");
  const [termsAgreement, setTermsAgreement] = useState<boolean>(false);
  const [selectboxData, setSelectboxData] = useState<SectorModel[] | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    init();
  }, []);

  useEffect(() => {
    if (currentSessionData) {
      setSelectedSectorId(currentSessionData.sectorId.toString() || "");
      setInsertedFullName(currentSessionData.fullName);
      setTermsAgreement(currentSessionData.agreement);
    }
  }, [currentSessionData]);

  const init = async () => {
    if (id) {
      const personData = await GetPersonById(id);
      if (typeof personData === "string") {
        setErrorMessage(personData);
        setTimeout(() => setErrorMessage(null), 2500);
      } else {
        setCurrentSessionData(personData);
      }
    }

    const sectorData = await GetAllSectors();
    if (typeof sectorData === "string") {
      setErrorMessage(sectorData);
    } else {
      setSelectboxData(sectorData);
    }
  };

  const isFormValid = () => insertedFullName !== "" && selectedSectorId !== "" && termsAgreement !== false;

  const flattenSectors = (nodes: SectorModel[], path = [] as string[]): DropdownModel[] => {
    let result: DropdownModel[] = [];

    for (let index = 0; index < nodes.length; index++) {
      const node = nodes[index];

      const newPath = path.concat((index + 1).toString());

      result.push({
        value: node.id.toString(),
        label: `${newPath.join(".")} ${node.name}`,
        indentLevel: path.length,
      });

      if (node.children && node.children.length > 0) {
        const childNodes = flattenSectors(node.children, newPath);
        for (let i = 0; i < childNodes.length; i++) {
          result.push(childNodes[i]);
        }
      }
    }
    return result;
  };

  const dropdownOptions = selectboxData ? flattenSectors(selectboxData) : [];

  const handleAdd = async () => {
    const data: PersonModel = {
      fullName: insertedFullName,
      sectorId: Number(selectedSectorId),
      agreement: termsAgreement,
    };

    const result = await AddPerson(data);
    if (result.success === true) {
      setSuccessMessage("Person added successfully");
      setTimeout(() => setSuccessMessage(null), 2500);
      setTimeout(() => navigate(`/${result.response}`), 2500);
    } else {
      setErrorMessage(String(result.errorMessage));
      setTimeout(() => setErrorMessage(null), 2500);
    }
  };

  const handleEdit = async () => {
    const data: PersonModel = {
      externalId: String(id),
      fullName: insertedFullName,
      sectorId: Number(selectedSectorId),
      agreement: termsAgreement,
    };

    const result = await UpdatePerson(data);
    if (result.success === true) {
      setSuccessMessage("Person updated successfully");
      setTimeout(() => setSuccessMessage(null), 2500);
      setTimeout(() => navigate(`/${result.response}`), 2500);
    } else {
      setErrorMessage(String(result.errorMessage));
      setTimeout(() => setErrorMessage(null), 2500);
    }
  };

  const handleDelete = async () => {
    const result = await RemovePerson(String(id));
    if (result.success === true) {
      setSuccessMessage("Person deleted successfully");
      setTimeout(() => setSuccessMessage(null), 2500);
      setTimeout(() => navigate(`/`), 2500);
    } else {
      setErrorMessage(String(result.errorMessage));
      setTimeout(() => setErrorMessage(null), 2500);
    }
  };

  return (
    <>
      <div className="flex max-h-screen max-w-screen items-center justify-center">
        <div className="flex flex-col gap-5">
          <div className="flex flex-col max-md:w-90 md:w-xl bg-main-dark rounded-3xl p-6 gap-15 items-center">
            <span className="text-2xl font-bold self-start">{id ? "Edit data" : "Add data"}</span>
            <div className="flex flex-col self-center items-center max-w-10/12 gap-5">
              <TextBox
                label={"Full name:"}
                icon="person-icon"
                value={insertedFullName}
                onChange={setInsertedFullName}
              />
              <DropDownList
                icon="work-icon"
                label="Select a sector"
                options={dropdownOptions}
                value={selectedSectorId}
                onChange={(e) => setSelectedSectorId(e.target.value)}
              />
              <Checkbox
                label={"I agree to terms"}
                checked={termsAgreement}
                onChange={(checked) => setTermsAgreement(checked)}
              />
              {errorMessage && <ErrorMessage text={errorMessage} />}
              {successMessage && <SuccessMessage text={successMessage} />}
            </div>
            <NormalButton
              text={id ? "Edit data" : "Add data"}
              onClick={() => {
                id ? handleEdit() : handleAdd();
              }}
              isDisabled={!isFormValid()}
            />
            {id && <NormalLink text="Remove data" onClick={handleDelete} />}
          </div>
        </div>
      </div>
    </>
  );
}

export default EditView;
