import axios from "axios";
import { IInvoice } from "@/types/invoice";

export class InvoiceService {
  API_URL = process.env.VUE_APP_API_ENDPOINT;

  public async makeNewInvoice(invoice: IInvoice): Promise<boolean> {
    let now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;
    let result: any = await axios.post(`${this.API_URL}/invoice/`, invoice);
    return result.data;
  }
}
