<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">
      Inventory Dashboard
    </h1>
    <hr />
    <div class="inventory-actions">
      <SolarButton @click.native="showProductModal" id="newItem"
        >Add New Item</SolarButton
      >
      <SolarButton @button:click="showShipmentModal" id="newShipment">
        Receive Shipment</SolarButton
      >
    </div>
    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td :class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">
          {{ item.quantityOnHand }}
        </td>
        <td>
          {{ item.product.price | price }}
        </td>
        <td>
          <span v-if="item.product.isTaxable">
            Yes
          </span>
          <span v-else>
            No
          </span>
        </td>
        <td>
          <div
            class="lni lni-cross-circle product-archive"
            @click="archiveProduct(item.product.id)"
          ></div>
        </td>
      </tr>
    </table>

    <NewProductModal
      v-if="isShowNewProductModal"
      @save:product="saveNewProduct"
      @close="closeModal"
    ></NewProductModal>
    <NewShipmentModal
      v-if="isShowNewShipmentModal"
      :inventory="inventory"
      @save:shipment="saveNewShipment"
      @close="closeModal"
    ></NewShipmentModal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import NewShipmentModal from "@/components/modals/NewShipmentModal.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import { IShipment } from "@/types/shipment";
import { IProduct } from "@/types/product";
import { InventoryService } from "@/services/inventory.service";
import { ProductService } from "@/services/product.service";

const inventoryService = new InventoryService();
const productService = new ProductService();

@Component({
  name: "Inventory",
  components: {
    SolarButton,
    NewProductModal,
    NewShipmentModal
  }
})
export default class Inventory extends Vue {
  isShowNewProductModal = false;
  isShowNewShipmentModal = false;
  inventory = [
    {
      id: 1,
      product: {
        name: "Some Product",
        description: "Description 1",
        price: 100,
        createdOn: new Date(),
        updatedOn: new Date(),
        isTaxable: true,
        isArchived: false
      },
      quantityOnHand: 100,
      idealQuantity: 100
    },
    {
      id: 2,
      product: {
        name: "Some Product 2",
        description: "Description 2",
        price: 100,
        createdOn: new Date(),
        updatedOn: new Date(),
        isTaxable: true,
        isArchived: false
      },
      quantityOnHand: 100,
      idealQuantity: 100
    }
  ];

  async created() {
    await this.fetchInventory();
  }

  async fetchInventory() {
    this.inventory = await inventoryService.getInventory();
  }

  showProductModal() {
    this.isShowNewProductModal = true;
  }

  showShipmentModal() {
    this.isShowNewShipmentModal = true;
  }

  async saveNewProduct(product: IProduct) {
    await productService.save(product);
    await this.fetchInventory();
  }

  async archiveProduct(productId: number) {
    await productService.archive(productId);
    await this.fetchInventory();
  }

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateShipmentQuantity(shipment);
    this.isShowNewShipmentModal = false;
    await this.fetchInventory();
  }

  applyColor(quantityOnHand: number, idealQuantity: number) {
    if (quantityOnHand <= 0) {
      return "red";
    }
    if (Math.abs(idealQuantity - quantityOnHand) > 8) {
      return "yellow";
    }
    return "green";
  }

  closeModal() {
    this.isShowNewShipmentModal = false;
    this.isShowNewProductModal = false;
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.green {
  font-weight: bold;
  color: $solar-green;
}
.yellow {
  font-weight: bold;
  color: $solar-yellow;
}
.red {
  font-weight: bold;
  color: $solar-red;
}
.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}
.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
