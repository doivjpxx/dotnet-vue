import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Home from "@/views/Home.vue";
import Inventory from "@/views/Inventory.vue";
import Customers from "@/views/Customers.vue";
import Orders from "@/views/Orders.vue";
import CreateInvoice from "@/views/CreateInvoice.vue";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Home",
    component: Home
  },
  {
    path: "/inventory",
    name: "Inventory",
    component: Inventory
  },
  {
    path: "/customers",
    name: "Customers",
    component: Customers
  },
  {
    path: "/orders",
    name: "Orders",
    component: Orders
  },
  {
    path: "/invoice/new",
    name: "create-invoice",
    component: CreateInvoice
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
