resource "kubernetes_namespace" "ittrap" {
  metadata {
    name = var.app_namespace

    labels = {
      "app.kubernetes.io/managed-by" = "terraform"
      "app.kubernetes.io/part-of"    = "ittrap"
    }
  }
}

resource "kubernetes_secret" "aws_account_info" {
  metadata {
    name      = "aws-account-info"
    namespace = kubernetes_namespace.ittrap.metadata[0].name

    labels = {
      "app.kubernetes.io/managed-by" = "terraform"
      "app.kubernetes.io/part-of"    = "ittrap"
    }
  }

  type = "Opaque"

  data = {
    account-id = data.aws_caller_identity.current.account_id
    region     = var.aws_region
  }
}