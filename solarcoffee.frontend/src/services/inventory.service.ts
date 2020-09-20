import axios from "axios";
import { IProductInventory } from "@/types/product";
import {IShipment} from "@/types/shipment";

export class InventoryService {
  API_URL = process.env.VUE_APP_API_ENDPOINT;

  public async getInventory(): Promise<IProductInventory[]> {
    let result = await axios.get(`${this.API_URL}/inventory/`);
    return result.data;
  }

  public async updateShipmentQuantity(shipment: IShipment): Promise<any> {
      let result = await axios.patch(`${this.API_URL}/inventory/`, shipment);
      return result.data;
  }
}
