import axios from "axios";
import SectorModel from "../Models/SectorModel";
import PersonModel from "../Models/PersonModel";
import ResponseModel from "../Models/ResponseModel";

export async function GetAllSectors(): Promise<SectorModel[] | string> {
  const response = await axios.get(
    `${import.meta.env.VITE_API_URL}/Person/Sectors`,
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
  if (response.status === 200 && !response.data.message) {
    const result: SectorModel[] = response.data;
    return result;
  }

  return response.data.message ?? "No internet connection";
}

export async function GetPersonById(
  personId: string
): Promise<PersonModel | string> {
  const response = await axios.get(
    `${import.meta.env.VITE_API_URL}/Person/${personId}`,
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
  if (response.status === 200 && !response.data.message) {
    const result: PersonModel = response.data;
    return result;
  }

  return response.data.message ?? "No internet connection";
}

export async function AddPerson(person: PersonModel): Promise<ResponseModel> {
  const response = await axios.post(
    `${import.meta.env.VITE_API_URL}/Person/Add`,
    {
      fullName: person.fullName,
      sectorId: person.sectorId,
      agreement: person.agreement,
      origin: "sectors-frontend",
    },
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  if (response.status === 200 && !response.data.message) {
    const result: ResponseModel = {
      success: true,
      response: response.data,
    };
    return result;
  }

  const result: ResponseModel = {
    success: false,
    errorMessage: response.data.message ?? "No internet connection",
  };
  return result;
}

export async function UpdatePerson(person: PersonModel): Promise<ResponseModel> {
  const response = await axios.patch(
    `${import.meta.env.VITE_API_URL}/Person/Update`,
    {
      externalId: person.externalId,
      fullName: person.fullName,
      sectorId: person.sectorId,
      agreement: person.agreement,
      origin: "sectors-frontend",
    },
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  if (response.status === 200 && !response.data.message) {
    const result: ResponseModel = {
      success: true,
      response: response.data,
    };
    return result;
  }

  const result: ResponseModel = {
    success: false,
    errorMessage: response.data.message ?? "No internet connection",
  };
  return result;
}

export async function RemovePerson(id: string): Promise<ResponseModel> {
  const response = await axios.delete(
    `${import.meta.env.VITE_API_URL}/Person/${id}`,
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );

  if (response.status === 200 && !response.data.message) {
    const result: ResponseModel = {
      success: true,
      response: response.data,
    };
    return result;
  }

  const result: ResponseModel = {
    success: false,
    errorMessage: response.data.message ?? "No internet connection",
  };
  return result;
}
