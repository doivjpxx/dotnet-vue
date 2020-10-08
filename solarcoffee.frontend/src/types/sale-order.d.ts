import { ICustomer } from "@/types/customer";
import { ILineItem } from "@/types/invoice";

export interface ISalesOrder {
  id: number;
  createdOn: Date;
  updatedOn?: Date;
  customer: ICustomer;
  isPaid: boolean;
  salesOrderItems: ILineItem[];
}
