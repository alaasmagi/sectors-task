interface PersonModel {
  externalId?: string;
  fullName: string;
  sectorId: number;
  agreement: boolean;
  origin?: string;
}

export default PersonModel;
