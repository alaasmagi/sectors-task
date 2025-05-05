import { useEffect, useState } from "react";
import "../../App.css";
import { useNavigate, useParams } from "react-router-dom";
import NormalMessage from "../UI components/NormalMessage";
import ContainerCardSmall from "../UI components/ContainerCardSmall";
import NormalButton from "../UI components/NormalButton";
import ErrorMessage from "../UI components/ErrorMessage";
import PersonModel from "../../Models/PersonModel";
import { GetPersonById } from "../../BusinessLogic/PersonData";

function MainView() {
  const { id } = useParams();
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [normalMessage, setNormalMessage] = useState<string | null>(null);
  const [currentSessionData, setCurrentSessionData] = useState<PersonModel | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    init();
  }, []);

  const init = async () => {
    if (id) {
      const personData = await GetPersonById(id);
      if (typeof personData === "string") {
        setErrorMessage(personData);
      } else {
        setCurrentSessionData(personData);
      }
    } else {
      setNormalMessage("No recent activity. Click the button below to add data.");
    }
  };

  return (
    <>
      <div className="flex max-h-screen max-w-screen items-center justify-center">
        <div className="flex flex-col gap-5">
          <div className="flex flex-col max-md:w-90 md:w-xl bg-main-dark rounded-3xl p-6 gap-5">
            <span className="text-2xl font-bold self-start">{"Recent activity"}</span>
            {currentSessionData && (
              <ContainerCardSmall
                boldLabelA={String(currentSessionData?.fullName)}
                linkText="Edit data"
                onClick={() => navigate(`/Edit/${id}`)}
              />
            )}
            <div className="flex flex-col self-center gap-3 max-w-xs">
              {normalMessage && <NormalMessage text={normalMessage} />}
              {errorMessage && <ErrorMessage text={errorMessage} />}
              <NormalButton text="Add data" onClick={() => navigate("/Edit")} />
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default MainView;
