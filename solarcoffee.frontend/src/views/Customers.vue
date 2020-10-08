<template>
  <div>
    <h1 id="customersTitle">
      Manage Customers
    </h1>
    <hr />
    <div class="customer-actions">
      <SolarButton @button:click="showNewCustomerModal">
        Add Customer
      </SolarButton>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>State</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>
          {{ customer.firstName + " " + customer.lastName }}
        </td>
        <td>
          {{
            customer.primaryAddress.addressLine1 +
              " " +
              customer.primaryAddress.addressLine2
          }}
        </td>
        <td>
          {{ customer.primaryAddress.state }}
        </td>
        <td>
          {{ customer.createdOn | humanizeDate }}
        </td>
        <td>
          <div
            class="lni lni-cross-circle customer-delete"
            @click="deleteCustomer(customer.id)"
          ></div>
        </td>
      </tr>
    </table>

    <NewCustomerModal
      v-if="isShowNewCustomerModal"
      @save:customer="saveCustomer"
      @close="closeModal"
    ></NewCustomerModal>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import { ICustomer } from "@/types/customer";
import CustomerService from "@/services/customer.service";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";

const customerService = new CustomerService();

@Component({
  name: "Customers",
  components: { SolarButton, NewCustomerModal }
})
export default class Customers extends Vue {
  customers: ICustomer[] = [];
  isShowNewCustomerModal = false;

  async created() {
    await this.fetchCustomers();
  }

  async fetchCustomers() {
    this.customers = await customerService.getCustomers();
  }

  async deleteCustomer(customerId: number) {
    await customerService.deleteCustomer(customerId);
    await this.fetchCustomers();
  }

  async saveCustomer(customer: ICustomer) {
    await customerService.addCustomer(customer);
    this.isShowNewCustomerModal = false;
    await this.fetchCustomers();
  }

  showNewCustomerModal() {
    this.isShowNewCustomerModal = true;
  }

  closeModal() {
    this.isShowNewCustomerModal = false;
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.customer-actions {
  display: flex;
  margin-bottom: 0.8rem;
  div {
    margin-right: 0.8rem;
  }
}
.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
