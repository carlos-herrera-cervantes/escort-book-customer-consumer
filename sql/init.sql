CREATE TABLE "public"."profile_status_category" (
    "id" varchar(100) NOT NULL,
    "name" varchar(100) NOT NULL,
    "active" bool NOT NULL DEFAULT true,
    "created_at" timestamp NOT NULL,
    "updated_at" timestamp NOT NULL,
    PRIMARY KEY ("id")
);

CREATE TABLE "public"."profile" (
    "id" varchar(100) NOT NULL,
    "customer_id" varchar(100) NOT NULL UNIQUE,
    "first_name" varchar(100),
    "last_name" varchar(100),
    "email" varchar(100) NOT NULL,
    "phone_number" varchar(20),
    "gender" varchar(20),
    "birthdate" date,
    "created_at" timestamp NOT NULL,
    "updated_at" timestamp NOT NULL,
    PRIMARY KEY ("id")
);

CREATE TABLE "public"."profile_status" (
    "id" varchar(100) NOT NULL,
    "customer_id" varchar(100) NOT NULL,
    "profile_status_category_id" varchar(100) NOT NULL,
    "created_at" timestamp NOT NULL,
    "updated_at" timestamp NOT NULL,
    CONSTRAINT "profile_status_customer_id_fkey" FOREIGN KEY ("customer_id") REFERENCES "public"."profile"("customer_id") ON DELETE CASCADE,
    CONSTRAINT "profile_status_profile_status_category_id_fkey" FOREIGN KEY ("profile_status_category_id") REFERENCES "public"."profile_status_category"("id"),
    PRIMARY KEY ("id")
);
