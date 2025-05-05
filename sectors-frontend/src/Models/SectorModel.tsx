interface SectorModel {
  id: number;
  name: string;
  children?: SectorModel[];
}

export default SectorModel;
